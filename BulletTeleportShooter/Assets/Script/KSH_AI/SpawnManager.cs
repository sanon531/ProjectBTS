using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SpawnType
{
    BREAK = 0,
    NORMAL,
    WAVE,
    BOSS,
    REWARD
}


[System.Serializable]
public class SpawnSequence
{
    public SpawnType spawnType;
    public float time;
    public int spawnCount;
    public Transform[] spawnPoint;
    public Enemy[] spawnEnemy;
    public string rewardString;
}
public class SpawnManager : MonoBehaviour
{
    private static int MAX_OBJECT_COUNT = 200;
    public static SpawnManager Instance { get; private set; } = null;
    [Header("- Spawn Sequence")]
    [SerializeField] private bool isLoop;
    [SerializeField] private SpawnSequence[] spawnSequence;
    [Header("- Properties")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [Header("- Power Up Properties")]
    [SerializeField] private float defaultTime;
    [SerializeField] private float powerUpDelay;
    [Header("- Power Up Values")]
    [SerializeField] private int powerUpHP;
    [SerializeField] private int powerUpATK;
    [SerializeField] private float powerUpSPD;
    [Header("- Power Up Priority")]
    [SerializeField] private int priorityHP;
    [SerializeField] private int priorityATK;
    [SerializeField] private int prioritySPD;
    private int priority { get => priorityATK + priorityHP + prioritySPD; }
    [Header("- Current Power Up Status")]
    [SerializeField] private int currentPowerUpHP;
    [SerializeField] private int currentPowerUpATK;
    [SerializeField] private float currentPowerUpSPD;

    [SerializeField] private int spawnedEnemyCount;

    private Transform playerTransform;
    private Sequence powerUpSequence;
    private int powerUpToken;
    private Dictionary<System.Type, Queue<Enemy>> objectPool;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        objectPool = new Dictionary<System.Type, Queue<Enemy>>();
        HashSet<System.Type> objectPoolChecker = new HashSet<System.Type>();
        for(int i = 0; i < spawnSequence.Length; ++i)
        {
            Enemy[] spawnableEnemy = spawnSequence[i].spawnEnemy;
            for(int j = 0; j < spawnableEnemy.Length; ++j)
            {
                System.Type enemyType = spawnableEnemy[j].GetType();
                if (objectPoolChecker.Add(enemyType))
                {
                    Queue<Enemy> newQueue = new Queue<Enemy>(MAX_OBJECT_COUNT);
                    for (int k = 0; k < MAX_OBJECT_COUNT; ++k)
                    {
                        newQueue.Enqueue(Instantiate(spawnableEnemy[j], Vector3.zero, Quaternion.identity, transform).SetActive(false));
                    }
                    objectPool.Add(enemyType, newQueue);
                }
            }
        }

        spawnedEnemyCount = 0;
        
        Sequence tokenSequence = DOTween.Sequence();
        tokenSequence.
            OnStart(() =>
            {
                powerUpToken = 0;
            }).
            AppendInterval(defaultTime).
            AppendCallback(() =>
            {
                powerUpSequence = DOTween.Sequence();
                powerUpSequence.AppendCallback(() =>
                {
                    powerUpToken++;
                }).
                AppendInterval(powerUpDelay).
                SetLoops(-1);
            });
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (playerTransform == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            yield return null;
        }

        float currentTime;
        SpawnSequence currentSequence;

        for (int sequenceIndex = 0; sequenceIndex < spawnSequence.Length - 1; )
        {
            currentSequence = spawnSequence[sequenceIndex];
            currentTime = 0f;
            float waitTime = currentSequence.time / (float)currentSequence.spawnCount;

            switch (currentSequence.spawnType)
            {
                case SpawnType.NORMAL:
                    UIManager.Instance.MakeTitle($"일반적으로 스폰 됩니다.", 2f);

                    {
                        for (int currentSpawnCount = 0; currentSpawnCount < currentSequence.spawnCount; ++currentSpawnCount)
                        {
                            currentTime = 0f;

                            // 위치 기반 스폰 포인트 선별
                            float min = minDistance * minDistance;
                            float max = maxDistance * maxDistance;
                            List<Vector3> spawnPos = new List<Vector3>();
                            for (int i = 0; i < currentSequence.spawnPoint.Length; ++i)
                            {
                                float sqrDistance = (playerTransform.position - currentSequence.spawnPoint[i].position).sqrMagnitude;
                                if (min <= sqrDistance && sqrDistance <= max)
                                {
                                    spawnPos.Add(currentSequence.spawnPoint[i].position);
                                }
                            }

                            if (spawnPos.Count > 0)
                            {
                                // 몇 마리를 스폰할 지 랜덤으로 결정
                                int spawnCount = Random.Range(0, spawnPos.Count / 2) + 1;
                                if (spawnedEnemyCount + spawnCount > MAX_OBJECT_COUNT) spawnCount = MAX_OBJECT_COUNT - spawnedEnemyCount;

                                // 어느 곳에 스폰할 지 랜덤으로 결정 (중복 불가)
                                int[] spawnPointIndex = new int[spawnPos.Count];
                                for (int i = 0; i < spawnPointIndex.Length; ++i) spawnPointIndex[i] = i;
                                for (int i = 0; i < spawnPointIndex.Length - 1; ++i)
                                {
                                    int randIndex = Random.Range(0, spawnPointIndex.Length);
                                    int tempValue = spawnPointIndex[i];
                                    spawnPointIndex[i] = spawnPointIndex[randIndex];
                                    spawnPointIndex[randIndex] = tempValue;
                                }

                                // 어떤 적을 스폰할 지 랜덤으로 결정 (중복 가능)
                                int[] spawnEnemyIndex = new int[spawnCount];
                                for (int i = 0; i < spawnCount; ++i) spawnEnemyIndex[i] = Random.Range(0, currentSequence.spawnEnemy.Length);

                                for (int i = 0; i < spawnCount; ++i)
                                {
                                    SpawnEnemy(currentSequence.spawnEnemy[spawnEnemyIndex[i]], spawnPos[spawnPointIndex[i]]);
                                }
                                spawnedEnemyCount += spawnCount; 
                                
                                while(currentTime < waitTime)
                                {
                                    if (spawnedEnemyCount <= 0) break;
                                    currentTime += Time.deltaTime;
                                    yield return null;
                                }
                            }
                            else yield return null;
                        }
                        break;
                    }
                case SpawnType.WAVE:
                    UIManager.Instance.MakeTitle($"적들이 한꺼번에 밀려옵니다.", 2f);
                    {
                        for (int currentSpawnCount = 0; currentSpawnCount < currentSequence.spawnCount; ++currentSpawnCount)
                        {
                            currentTime = 0f;

                            List<Vector3> spawnPos = new List<Vector3>();
                            for (int i = 0; i < currentSequence.spawnPoint.Length; ++i)
                            {
                                spawnPos.Add(currentSequence.spawnPoint[i].position);
                            }

                            if (spawnPos.Count > 0)
                            {
                                for (int i = 0; i < spawnPos.Count; ++i)
                                {
                                    SpawnEnemy(currentSequence.spawnEnemy[Random.Range(0, currentSequence.spawnEnemy.Length)], spawnPos[i]);
                                }
                                spawnedEnemyCount += spawnPos.Count;
                                while (currentTime < waitTime)
                                {
                                    if (spawnedEnemyCount <= 0) break;
                                    currentTime += Time.deltaTime;
                                    yield return null;
                                }
                            }
                            else yield return null;
                        }
                        break;
                    }
                case SpawnType.BOSS:
                    UIManager.Instance.MakeTitle($"관리자가 등장했습니다.", 2f);

                    {
                        currentTime = 0f;

                        Enemy newEnemy = Instantiate(currentSequence.spawnEnemy[0], currentSequence.spawnPoint[0].position, Quaternion.identity).Init();
                        newEnemy.Attack += currentPowerUpATK;
                        newEnemy.MaxHP += currentPowerUpHP;
                        newEnemy.Speed += currentPowerUpSPD;
                        newEnemy.onDeath += () =>
                        {
                            Sequence sequence = DOTween.Sequence();
                            sequence.
                            AppendInterval(2f).
                            AppendCallback(() =>
                            {
                                spawnedEnemyCount--;
                                Debug.Log(newEnemy.name + " DEAD");
                            });
                        };
                        Color outlineColor = Color.white;
                        if (0 <= priority && priority < 7)
                        {
                            outlineColor = Color.white;
                        }
                        else if (7 <= priority && priority < 10)
                        {
                            outlineColor = new Color(0.5f, 0.75f, 0.27f, 1f);
                        }
                        else if (10 <= priority && priority < 14)
                        {
                            outlineColor = Color.green;
                        }
                        else if (14 <= priority && priority < 18)
                        {
                            outlineColor = Color.blue;
                        }
                        else if (18 <= priority && priority < 22)
                        {
                            outlineColor = Color.magenta;
                        }
                        else if (22 <= priority)
                        {
                            outlineColor = Color.red;
                        }
                        newEnemy.Outline.color = outlineColor;
                        spawnedEnemyCount++;
                        while (currentTime < waitTime)
                        {
                            if (spawnedEnemyCount <= 0) break;
                            currentTime += Time.deltaTime;
                            yield return null;
                        }
                        break;
                    }
                case SpawnType.BREAK:
                    UIManager.Instance.MakeTitle($"잠시 쉬어갑니다.", 2f);
                    {
                        if (currentSequence.time > 0)
                        {
                            yield return new WaitForSeconds(currentSequence.time);
                        }
                        break;
                    }
                case SpawnType.REWARD:
                    UIManager.Instance.MakeTitle($"{currentSequence.rewardString}총을 획득했습니다.", 2f);
                    if (currentSequence.time > 0)
                    {
                        yield return new WaitForSeconds(currentSequence.time);
                    }
                    Debug.Log("reward Get");
                    break;

            }

            if (powerUpToken > 0)
            {
                for (; powerUpToken > 0; --powerUpToken)
                {
                    int random = Random.Range(0, priority) + 1;
                    if (1 <= random && random <= priorityATK)
                    {
                        priorityHP += 2;
                        prioritySPD += 2;
                        currentPowerUpATK += powerUpATK;
                    }
                    else if (priorityATK < random && random <= priorityATK + priorityHP)
                    {
                        priorityATK += 2;
                        prioritySPD += 2;
                        currentPowerUpHP += powerUpHP;
                    }
                    else if (priorityATK + priorityHP < random && random <= priorityATK + priorityHP + prioritySPD)
                    {
                        priorityATK += 2;
                        priorityHP += 2;
                        currentPowerUpSPD += powerUpSPD;
                    }
                }
                UIManager.Instance.MakeNotice($"적이 강해졌습니다!\nATK {currentPowerUpATK / powerUpATK}++ SPD {currentPowerUpSPD / powerUpSPD}++ HP {currentPowerUpHP / powerUpHP}++", 2f);
            }

            sequenceIndex++;
            if (isLoop && sequenceIndex >= spawnSequence.Length - 1) sequenceIndex = 0;
            yield return null;
        }

        void SpawnEnemy(Enemy _spawnEnemy, Vector3 _position)
        {
            Enemy newEnemy = objectPool[_spawnEnemy.GetType()].Dequeue().SetActive(true).SetPosition(_position).Init();
            newEnemy.Attack += currentPowerUpATK;
            newEnemy.MaxHP += currentPowerUpHP;
            newEnemy.Speed += currentPowerUpSPD;
            newEnemy.onDeath += () =>
            {
                Sequence sequence = DOTween.Sequence();
                sequence.
                AppendInterval(2f).
                AppendCallback(() =>
                {
                    spawnedEnemyCount--;
                    objectPool[newEnemy.GetType()].Enqueue(newEnemy.SetActive(false));
                    //Debug.Log(newEnemy.name + " DEAD");
                });
            };
            Color outlineColor = Color.white;
            if (0 <= priority && priority < 7)
            {
                outlineColor = Color.white;
            }
            else if (7 <= priority && priority < 10)
            {
                outlineColor = new Color(0.5f, 0.75f, 0.27f, 1f);
            }
            else if (10 <= priority && priority < 14)
            {
                outlineColor = Color.green;
            }
            else if (14 <= priority && priority < 18)
            {
                outlineColor = Color.blue;
            }
            else if (18 <= priority && priority < 22)
            {
                outlineColor = Color.magenta;
            }
            else if (22 <= priority)
            {
                outlineColor = Color.red;
            }
            newEnemy.Outline.color = outlineColor;
        }

    }
}

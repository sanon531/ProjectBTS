using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; } = null;
    [Header("- Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("- Spawn Enemies")]
    [SerializeField] private Enemy[] spawnEnemies;
    [Header("- Properties")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private int MAX_ENEMY_COUNT;
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
    [Header("- Current Power Up Status")]
    [SerializeField] private int currentPowerUpHP;
    [SerializeField] private int currentPowerUpATK;
    [SerializeField] private float currentPowerUpSPD;

    private int spawnedEnemyCount;
    private Transform playerTransform;
    private Sequence powerUpSequence;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Sequence waitSequence = DOTween.Sequence();
        waitSequence.
            AppendInterval(defaultTime).
            AppendCallback(() =>
            {
                powerUpSequence = DOTween.Sequence();
                powerUpSequence.AppendCallback(() =>
                {
                    int random = Random.Range(0, priorityATK + priorityHP + prioritySPD) + 1;
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

        while (spawnedEnemyCount < MAX_ENEMY_COUNT)
        {
            // 위치 기반 스폰 포인트 선별
            float min = minDistance * minDistance;
            float max = maxDistance * maxDistance;
            List<Vector3> spawnPos = new List<Vector3>();
            for(int i = 0; i < spawnPoints.Length; ++i)
            {
                float sqrDistance = (playerTransform.position - spawnPoints[i].position).sqrMagnitude;
                if (min <= sqrDistance && sqrDistance <= max)
                {
                    spawnPos.Add(spawnPoints[i].position);
                }
            }

            if(spawnPos.Count > 0)
            {
                // 몇 마리를 스폰할 지 랜덤으로 결정
                int spawnCount = Random.Range(0, spawnPos.Count) + 1;
                if (spawnedEnemyCount + spawnCount > MAX_ENEMY_COUNT) spawnCount = MAX_ENEMY_COUNT - spawnedEnemyCount;

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
                for (int i = 0; i < spawnCount; ++i) spawnEnemyIndex[i] = Random.Range(0, spawnEnemies.Length);

                // 적을 스폰
                for (int i = 0; i < spawnCount; ++i)
                {
                    Enemy newEnemy = Instantiate(spawnEnemies[spawnEnemyIndex[i]], spawnPos[spawnPointIndex[i]], Quaternion.identity, transform).Init();
                    newEnemy.Attack += currentPowerUpATK;
                    newEnemy.MaxHP += currentPowerUpHP;
                    newEnemy.Speed += currentPowerUpSPD;
                }

                spawnedEnemyCount += spawnCount;
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSSpawnItem : MonoBehaviour
{

    /*
     * 
     * 
   // 이 친구는 뭐지
    public static BTSSpawnItem Instance { get; private set; } = null;

    private static int MAX_OBJECT_COUNT = 10;


    [Header("- Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("- Spawn Items")]
    //[SerializeField] private Enemy[] spawnItems; //왜 item 으로 했을땐 안돼지??
    [SerializeField] private List<BTSSpawnItem> Items = new List<BTSSpawnItem>();

    [Header("- Properties")]
    [SerializeField] private float spawnDelay;
    [SerializeField] private int MAX_ITEM_COUNT;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    [SerializeField] private int spawnedItemCount;
    private Transform playerTransform;
    private List<Queue<BTSSpawnItem>> objectPool;
    private Dictionary<System.Type, int> objectIndex;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        objectPool = new List<Queue<BTSSpawnItem>>(Items.Count);
        objectIndex = new Dictionary<System.Type, int>(Items.Count);
        int objectCount = Mathf.Min(MAX_ITEM_COUNT, (int)(MAX_ITEM_COUNT * 1.5f));
        for (int i = 0; i < Items.Count; ++i)
        {
            objectIndex.Add(Items[i].GetType(), i);
            Queue<BTSSpawnItem> newQueue = new Queue<BTSSpawnItem>(objectCount);
            for (int j = 0; j < objectCount; ++j)
            {
                newQueue.Enqueue(Instantiate(Items[i], Vector3.zero, Quaternion.identity, transform).SetActive(false));
            }
            objectPool.Add(newQueue);
        }

        spawnedItemCount = 0;
        StartCoroutine(SpawnRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        
        while (true)
        {
            if (spawnedItemCount < MAX_ITEM_COUNT)
            {
                // 위치 기반 스폰 포인트 선별
                float min = minDistance * minDistance;
                float max = maxDistance * maxDistance;
                List<Vector3> spawnPos = new List<Vector3>();
                for (int i = 0; i < spawnPoints.Length; ++i)
                {
                    float sqrDistance = (playerTransform.position - spawnPoints[i].position).sqrMagnitude;
                    if (min <= sqrDistance && sqrDistance <= max)
                    {
                        spawnPos.Add(spawnPoints[i].position);
                    }
                }

                if (spawnPos.Count > 0)
                {
                    // 몇 마리를 스폰할 지 랜덤으로 결정
                    // int spawnCount = Random.Range(0, spawnPos.Count) + 1;
                    int spawnCount = 1;
                    if (spawnedItemCount + spawnCount > MAX_ITEM_COUNT) spawnCount = MAX_ITEM_COUNT - spawnedItemCount;

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
                    int[] spawnItemIndex = new int[spawnCount];
                    for (int i = 0; i < spawnCount; ++i) spawnItemIndex[i] = Random.Range(0, Items.Count);

                    // 적을 스폰
                    for (int i = 0; i < spawnCount; ++i)
                    {
                        //Enemy newEnemy = Instantiate(spawnEnemies[spawnEnemyIndex[i]], spawnPos[spawnPointIndex[i]], Quaternion.identity, transform).Init();
                        BTSSpawnItem newItem = objectPool[spawnItemIndex[i]].Dequeue().SetActive(true).SetPosition(spawnPos[spawnPointIndex[i]]).Init();

                        /*
                         
                        여기는 아마 아이템 사라지는 조건이 필요할 듯 (밑에 코드가 보면, 적이 죽었을 때 SetActive(false)가 되기 때문)
                        
                         newItem.onDeath += () =>
                        {
                            Sequence sequence = DOTween.Sequence();
                            sequence.
                            AppendInterval(2f).
                            AppendCallback(() =>
                            {
                                spawnedItemCount--;
                                objectPool[objectIndex[newEnemy.GetType()]].Enqueue(newEnemy.SetActive(false));
                            });
                        };
                        //objectPool[objectIndex[newItem.GetType()]].Enqueue(newItem.SetActive(false));

                    }
                    spawnedItemCount += spawnCount;

                }
                yield return new WaitForSeconds(spawnDelay);
            }
            else yield return null;
        }
    }

            */
}

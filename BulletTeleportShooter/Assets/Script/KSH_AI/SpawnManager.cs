using System.Collections;
using UnityEngine;

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
    private int spawnedEnemyCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        WaitForSeconds waitForDelay = new WaitForSeconds(spawnDelay);
        while(spawnedEnemyCount < MAX_ENEMY_COUNT)
        {
            // 몇 마리를 스폰할 지 랜덤으로 결정
            int spawnCount = Random.Range(1, spawnPoints.Length);
            if (spawnedEnemyCount + spawnCount >= MAX_ENEMY_COUNT) spawnCount = MAX_ENEMY_COUNT - spawnedEnemyCount;

            // 어느 곳에 스폰할 지 랜덤으로 결정 (중복 불가)
            int[] spawnPointIndex = new int[spawnPoints.Length];
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
            for(int i = 0; i < spawnCount; ++i)
            {
                Instantiate(spawnEnemies[spawnEnemyIndex[i]], spawnPoints[spawnPointIndex[i]].position, Quaternion.identity).Init();
            }

            spawnedEnemyCount += spawnCount;

            yield return waitForDelay;
        }
    }

}

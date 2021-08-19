using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [Header("- Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;
    [Header("- Spawn Items")]
    //[SerializeField] private Enemy[] spawnItems; //왜 item 으로 했을땐 안돼지??
    //[SerializeField] private List Items = new List();
    [SerializeField] private List<GameObject> Items = new List<GameObject>();

    [Header("- Properties")]
    [SerializeField] private float spawnDelay;

    public Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnItems()
    {
        int temp;
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            int number = Random.Range(0, spawnPoints.Length);
            int num2 = Random.Range(0, 3);
            temp = num2;
            // 이거 한번에 소환하는법 여쭤보기 + 주인공 죽는거 아는법 -> 리셋해야되니까
            GameObject item = Instantiate(Items[num2], new Vector3(0,0,0) , Quaternion.identity);
            if (temp == num2)
            {
                item.transform.position = spawnPoints[number].transform.position + new Vector3(0, 1, 0);
            }
            else
            {
                item.transform.position = spawnPoints[number].transform.position;
            }
            temp = num2;
            Destroy(item, 40f);

        }
    }
}

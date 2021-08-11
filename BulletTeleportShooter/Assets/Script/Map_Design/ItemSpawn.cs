﻿using System.Collections;
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
    [SerializeField] private int MAX_ITEM_COUNT;

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
        while(true)
        {
            yield return new WaitForSeconds(spawnDelay);
            int number = Random.Range(0, spawnPoints.Length);
            int num2 = Random.Range(0, 3);

            //Instantiate(Items[num2], new Vector3(0, 0, 0), Quaternion.identity);
            GameObject item = Instantiate(Items[num2], new Vector3(0,0,0) , Quaternion.identity);
            item.transform.position = spawnPoints[number].transform.position;
        }
    }
}
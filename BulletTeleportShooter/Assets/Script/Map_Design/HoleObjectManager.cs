﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleObjectManager : MonoBehaviour
{
    public static HoleObjectManager Instance;

    [SerializeField]
    List<BTS_HoleObjectContainer> HoleContainerList = new List<BTS_HoleObjectContainer>();
    [SerializeField]
    List<BrokenObjectContainer> BrokenContainerList = new List <BrokenObjectContainer>();
    [SerializeField]
    List<BTS_DustEffect> DustEffectList = new List<BTS_DustEffect>();


    public float firstfall;
    public float secondfall;
    public float thirdfall;
    Coroutine coroutine;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DustEffectList[0].t = firstfall;
        DustEffectList[1].t = secondfall;
        DustEffectList[2].t = thirdfall;
    }
    private void Start()
    {

        StartCoroutine(FallTile(firstfall, 0));
        StartCoroutine(FallTile(secondfall, 1));
        StartCoroutine(FallTile(thirdfall, 2));
    }
    // Update is called once per frame
    void Update()
    { 

    }

    IEnumerator FallTile(float time, int num)
    {
        yield return new WaitForSeconds(time-5);
        BrokenContainerList[num].Play_Broken();
        yield return new WaitForSeconds(5);
        Destroy(HoleContainerList[num].gameObject);
    }
}

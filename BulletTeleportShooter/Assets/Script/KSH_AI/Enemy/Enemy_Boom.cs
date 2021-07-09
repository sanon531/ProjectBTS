using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boom : Enemy
{
    [SerializeField] private float defaultBoomDelay;
    [SerializeField] private int defaultBoomDamage;

    public float BoomDelay { get; private set; }
    public int BoomDamage { get; private set; }

    public override Enemy Init()
    {
        BoomDelay = defaultBoomDelay;
        BoomDamage = defaultBoomDamage;


        return base.Init();
    }

    private void Start()
    {
        if (IsInit == false)
        {
            Init();
        }
    }

}

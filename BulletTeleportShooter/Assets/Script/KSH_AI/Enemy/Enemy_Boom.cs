using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boom : Enemy
{
    [SerializeField] private float defaultBoomDelay;
    [Header("- Current Status")]
    [SerializeField] private int currentMaxHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentBoomDelay;

    public float BoomDelay { get; private set; }

    public override Enemy Init()
    {
        BoomDelay = defaultBoomDelay;
        return base.Init();
    }

    private void Update()
    {
        currentMaxHP = MaxHP;
        currentAttack = Attack;
        currentSpeed = Speed;
        currentBoomDelay = BoomDelay;
    }
}

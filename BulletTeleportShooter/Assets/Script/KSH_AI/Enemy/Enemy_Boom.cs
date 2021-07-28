using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boom : Enemy
{
    [SerializeField] private float defaultBoomDelay;
    [SerializeField] private float defaultBoomRadius;
    [Header("- Current Status")]
    [SerializeField] private int currentMaxHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentBoomDelay;
    [SerializeField] private float currentBoomRadius;

    public float BoomDelay { get; private set; }
    public float BoomRadius { get; private set; }

    public override Enemy Init()
    {
        BoomDelay = defaultBoomDelay;
        BoomRadius = defaultBoomRadius;
        return base.Init();
    }

    private void Update()
    {
        currentMaxHP = MaxHP;
        currentAttack = Attack;
        currentSpeed = Speed;
        currentBoomDelay = BoomDelay;
        currentBoomRadius = BoomRadius;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dash : Enemy
{
    [SerializeField] private float defaultReadyTime;
    [SerializeField] private float defaultDashDelay;
    [Header("- Current Status")]
    [SerializeField] private int currentMaxHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentReadyTime;
    [SerializeField] private float currentDashDelay;

    public float DashReady { get; private set; }
    public float DashDelay { get; private set; }

    public override Enemy Init()
    {
        DashReady = defaultReadyTime;
        DashDelay = defaultDashDelay;
        return base.Init();
    }

    private void Update()
    {
        currentMaxHP = MaxHP;
        currentAttack = Attack;
        currentSpeed = Speed;
        currentReadyTime = DashReady;
        currentDashDelay = DashDelay;
    }
}

using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [SerializeField] private float defaultDashTime;
    [SerializeField] private float defaultLaserTime;        
    [SerializeField] private float defaultLaserDelay;
    [SerializeField] private int defaultLaserAttack;
    [SerializeField] private float defaultLaserRadius;

    [Header("- Current Status")]
    [SerializeField] private int currentMaxHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentDashTime;
    [SerializeField] private float currentLaserTime;
    [SerializeField] private float currentLaserDelay;
    [SerializeField] private int currentLaserAttack;
    [SerializeField] private float currentLaserRadius;

    [Header("- Feedbacks")]
    [SerializeField] private MMFeedbacks laserFeedback;
    public MMFeedbacks LaserFeedback { get => laserFeedback; }

    public float DashTime { get; private set; }             // 대쉬하는데 걸리는 시간
    public float LaserTime { get; private set; }            // 레이저를 쏘는데 걸리는 시간
    public float LaserDelay { get; private set; }           // 레이저를 쏘고 다음 레이저를 쏘기까지의 시간
    public int LaserAttack { get; private set; }          // 레이저의 대미지
    public float LaserRadius { get; private set; }          // 레이저 폭발 반경

    public override Enemy Init()
    {
        DashTime = defaultDashTime;
        LaserTime = defaultLaserTime;
        LaserDelay = defaultLaserDelay;
        LaserAttack = defaultLaserAttack;
        LaserRadius = defaultLaserRadius;
        LaserFeedback?.Initialization(this.gameObject);
        return base.Init();
    }

    private void Update()
    {
        currentMaxHP = MaxHP;
        currentAttack = Attack;
        currentSpeed = Speed;

        currentDashTime = DashTime;
        currentLaserTime = LaserTime;
        currentLaserDelay = LaserDelay;
        currentLaserAttack = LaserAttack;
        currentLaserRadius = LaserRadius;

    }
}

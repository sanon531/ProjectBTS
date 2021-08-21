using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

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
    [Header("- Feedbacks")]
    [SerializeField] private MMFeedbacks boomFeedback;

    public float BoomDelay { get; private set; }
    public float BoomRadius { get; private set; }
    public MMFeedbacks BoomFeedback { get => boomFeedback; }

    public override Enemy Init()
    {
        BoomDelay = defaultBoomDelay;
        BoomRadius = defaultBoomRadius;
        BoomFeedback?.Initialization(this.gameObject);
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

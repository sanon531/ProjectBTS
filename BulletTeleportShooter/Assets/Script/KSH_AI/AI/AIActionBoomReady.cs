using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionBoomReady : AIAction
{
    private Enemy_Boom enemy;
    [Header("- Area")]
    [SerializeField] private Transform dashArea;
    [SerializeField] private Transform dashReadyArea;
    [SerializeField] private float range;
    [Header("- Time")]
    [SerializeField] private float readyTime;
    [Header("- Checker")]
    [SerializeField] private AIDecisionCheck checker;

    public override void PerformAction()
    {

    }

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boom>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        readyTime = enemy.BoomDelay;

        dashArea.gameObject.SetActive(true);
        dashArea.localScale = new Vector3(range * 2, range * 2, 1);
        dashReadyArea.localScale = new Vector3(0, 0, 1);

        Sequence readySequence = DOTween.Sequence();
        readySequence.
            Append(dashReadyArea.DOScaleX(1, readyTime)).
            Join(dashReadyArea.DOScaleY(1, readyTime)).
            SetEase(Ease.Linear).
            OnComplete(() => checker.checker = true);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        dashArea.gameObject.SetActive(false);
    }
}

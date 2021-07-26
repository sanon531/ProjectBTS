using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionBoomReady : AIAction
{
    private Enemy_Boom enemy;
    private Sequence readySequence;
    [Header("- Area")]
    [SerializeField] private Transform boomArea;
    [SerializeField] private Transform boomReadyArea;
    [SerializeField] private float range;
    [Header("- Time")]
    [SerializeField] private float readyTime;
    [Header("- Checker")]
    [SerializeField] private AIDecisionCheck checker;

    public override void PerformAction()
    {
        if (enemy.CurrentHP <= 0)
        {
            if (readySequence.IsActive())
            {
                readySequence.Kill();
                boomArea.gameObject.SetActive(false);
            }
        }
    }

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boom>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        readyTime = enemy.BoomDelay;

        boomArea.gameObject.SetActive(true);
        boomArea.localScale = new Vector3(range * 2, range * 2, 1);
        boomReadyArea.localScale = new Vector3(0, 0, 1);

        readySequence = DOTween.Sequence();
        readySequence.
            Append(boomReadyArea.DOScaleX(1, readyTime)).
            Join(boomReadyArea.DOScaleY(1, readyTime)).
            SetEase(Ease.Linear).
            OnComplete(() => checker.checker = true);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        if (readySequence.IsActive())
        {
            readySequence.Kill();
        }
        boomArea.gameObject.SetActive(false);
    }
}

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
    [Header("- Alarm")]
    [SerializeField] private GameObject alarm;
    [Header("- Checker")]
    [SerializeField] private AIDecisionCheck checker;

    public override void PerformAction()
    {
        if (enemy.CurrentHP <= 0)
        {
            if (readySequence.IsActive())
            {
                readySequence.Kill();
                alarm.SetActive(false);
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
        float delay = enemy.BoomDelay;
        float radius = enemy.BoomRadius;

        alarm.SetActive(true);
        boomArea.gameObject.SetActive(true);
        boomArea.localScale = new Vector3(radius * 2, radius * 2, 1);
        boomReadyArea.localScale = new Vector3(0, 0, 1);

        readySequence = DOTween.Sequence();
        readySequence.
            Append(boomReadyArea.DOScaleX(1, delay)).
            Join(boomReadyArea.DOScaleY(1, delay)).
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
        alarm.SetActive(false);
        boomArea.gameObject.SetActive(false);
    }
}

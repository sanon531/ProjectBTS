using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionDashReady : AIAction
{
    private Enemy_Dash enemy;
    private Tweener dashTweener;
    [Header("- Area")]
    [SerializeField] private Transform dashArea;
    [SerializeField] private Transform dashReadyArea;
    [SerializeField] private float range;
    [Header("- Checker")]
    [SerializeField] private AIDecisionCheck checker;
    [Header("- Dash AI")]
    [SerializeField] private AIActionDashTarget dash;

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Dash>();
    }

    public override void PerformAction()
    {
        if(enemy.CurrentHP <= 0)
        {
            if (dashTweener.IsActive())
            {
                dashTweener.Kill();
                dashArea.gameObject.SetActive(false);
            }
        }
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        dashArea.gameObject.SetActive(true);
        dash.destination = _brain.Target.position;
        dashArea.rotation = Quaternion.Euler(
            new Vector3(0, 0,
            Vector2.Angle(_brain.Target.position - transform.position, Vector2.right) * (_brain.Target.position.y > transform.position.y ? 1f : -1f)));
        dashArea.localScale = new Vector3(range, 1, 1);
        dashReadyArea.localScale = new Vector3(0, 1, 1);
        dashTweener = 
        dashReadyArea.
            DOScaleX(1, enemy.DashReady).
            SetEase(Ease.Linear).
            OnComplete(() => checker.checker = true);
    }

    public override void OnExitState()
    {
        base.OnExitState();
        dashTweener = null;
        dashArea.gameObject.SetActive(false);
    }
}

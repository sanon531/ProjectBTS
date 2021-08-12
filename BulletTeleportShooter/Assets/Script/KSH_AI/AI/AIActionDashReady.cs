using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
[System.Serializable]
public enum DashReadyMode
{
    None = 0,
    ReadyUntilDelay,
    ReadyUntilFrame
}

public class AIActionDashReady : AIAction
{
    private Enemy_Dash enemy;
    protected Character _character;
    private Tweener dashTweener;
    [SerializeField] private DashReadyMode dashReadyMode;
    [Header("- Area")]
    [SerializeField] private Transform dashArea;
    [SerializeField] private Transform dashReadyArea;
    [SerializeField] private float range;
    [Header("- Alarm")]
    [SerializeField] private GameObject alarm;
    [Header("- Checker")]
    [SerializeField] private AIDecisionCheck checker;
    [Header("- Dash AI")]
    [SerializeField] private AIActionDashTarget dash;

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Dash>();
        _character = GetComponentInParent<Character>();
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
            StopAllCoroutines();
        }
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        dash.destination = _brain.Target.position;
        _character.CharacterModel.transform.localRotation = Quaternion.Euler(0, _brain.Target.position.x > transform.position.x ? 0 : 180, 0);

        switch (dashReadyMode)
        {
            case DashReadyMode.ReadyUntilDelay:
                {
                    alarm.SetActive(true);
                    dashArea.gameObject.SetActive(true);
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
                    break;
                }
                
            case DashReadyMode.ReadyUntilFrame:
                {
                    StartCoroutine(WaitFrame());
                    break;
                }
        }

        IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();
            checker.checker = true;
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        dashTweener = null;
        alarm.SetActive(false);
        dashArea.gameObject.SetActive(false);
    }
}

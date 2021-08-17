using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using UnityEngine;

public class AIActionBossDash : AIAction
{
    private Enemy_Boss enemy;
    private Tweener dashTweener;
    protected Character _character;
    protected CharacterDash2D _characterDamageDash2D; // 일단 DamageOnTouch가 구현되지 않았으므로 일반 Dash로 하자.
    [SerializeField] private DashReadyMode dashReadyMode;
    [Header("- Area")]
    [SerializeField] private Transform dashArea;
    [SerializeField] private Transform dashReadyArea;
    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        _character = GetComponentInParent<Character>();
        _characterDamageDash2D = _character?.FindAbility<CharacterDash2D>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        Vector3 destination = _brain.Target.position;
        float distance = 100;
        RaycastHit2D[] hit2D = Physics2D.CircleCastAll(transform.position, ((CircleCollider2D)enemy.Collider).radius, destination, distance);
        for (int i = 0; i < hit2D.Length; ++i)
        {
            if (hit2D[i].collider.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
            {
                distance = (hit2D[i].point - (Vector2)transform.position).magnitude;
                break;
            }
        }
        _character.CharacterModel.transform.localRotation = Quaternion.Euler(0, _brain.Target.position.x > transform.position.x ? 0 : 180, 0);
        _characterDamageDash2D.DashDistance = distance;
        _characterDamageDash2D.DashDuration = 2f;
        _characterDamageDash2D.DashDirection = (destination - this.transform.position);
        switch (dashReadyMode)
        {
            case DashReadyMode.ReadyUntilDelay:
                {
                    dashArea.gameObject.SetActive(true);
                    dashArea.rotation = Quaternion.Euler(
                        new Vector3(0, 0,
                        Vector2.Angle(_brain.Target.position - transform.position, Vector2.right) * (_brain.Target.position.y > transform.position.y ? 1f : -1f)));
                    dashArea.localScale = new Vector3(distance, 4, 1);
                    dashReadyArea.localScale = new Vector3(0, 1, 1);
                    dashTweener =
                    dashReadyArea.
                        DOScaleX(1, enemy.DashTime).
                        SetEase(Ease.Linear).
                        OnComplete(Dash);
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
            Dash();
        }

        void Dash()
        {
            dashArea.gameObject.SetActive(false);
            _characterDamageDash2D.DashStart();
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        dashTweener = null;
        dashArea.gameObject.SetActive(false);
    }

    public override void PerformAction()
    {
        if (enemy.CurrentHP <= 0)
        {
            if (dashTweener.IsActive())
            {
                dashTweener.Kill();
                dashArea.gameObject.SetActive(false);
            }
            StopAllCoroutines();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionDashTarget : AIAction
{
    public Vector3 destination;

    protected Character _character;
    protected CharacterDash2D _characterDamageDash2D; // 일단 DamageOnTouch가 구현되지 않았으므로 일반 Dash로 하자.

    [SerializeField] private AIDecisionCheck checker;

    protected override void Initialization()
    {
        _character = GetComponentInParent<Character>();
        _characterDamageDash2D = _character?.FindAbility<CharacterDash2D>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        _characterDamageDash2D.DashDirection = (destination - this.transform.position);
        _characterDamageDash2D.DashStart();
        Sequence aiSequence = DOTween.Sequence();
        aiSequence.AppendInterval(1.5f).AppendCallback(() => checker.checker = true);
    }
    public override void PerformAction()
    {
        
    }
}

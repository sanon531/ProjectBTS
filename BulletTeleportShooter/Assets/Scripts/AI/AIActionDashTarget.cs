using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionDashTarget : AIAction
{
    protected Character _character;
    protected CharacterDash2D _characterDamageDash2D; // 일단 DamageOnTouch가 구현되지 않았으므로 일반 Dash로 하자.

    protected override void Initialization()
    {
        _character = GetComponentInParent<Character>();
        _characterDamageDash2D = _character?.FindAbility<CharacterDash2D>();
    }
    public override void PerformAction()
    {
        _characterDamageDash2D.DashDirection = (_brain.Target.position - this.transform.position).normalized;
        _characterDamageDash2D.DashStart();
    }
}

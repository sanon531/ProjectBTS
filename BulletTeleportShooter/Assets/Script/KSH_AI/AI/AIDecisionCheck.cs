using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionCheck : AIDecision
{
    public bool checker;

    public override void OnEnterState()
    {
        base.OnEnterState();
        checker = false;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        checker = false;
    }

    public override bool Decide()
    {
        return checker;
    }
}

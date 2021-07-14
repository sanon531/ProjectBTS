using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionMyHit : AIDecision
{
    public int NumberOfHits = 1;

    protected int _hitCounter;
    protected Health _health;

    protected bool IsInit = false;

    public override void Initialization()
    {
        if (!IsInit)
        {
            _health = _brain.gameObject.GetComponentInParent<Health>();
            _health.OnHit += OnHit;
            _hitCounter = 0;
            IsInit = true;
        }
    }

    public override bool Decide()
    {
        return EvaluateHits();
    }

    protected virtual bool EvaluateHits()
    {
        return (_hitCounter >= NumberOfHits);
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        _hitCounter = 0;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _hitCounter = 0;
    }

    protected virtual void OnHit()
    {
        _hitCounter++;   
    }
    protected virtual void OnDisable()
    {
        if (_health != null)
        {
            _health.OnHit -= OnHit;
        }
    }

}

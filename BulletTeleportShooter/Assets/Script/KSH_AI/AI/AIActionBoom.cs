using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionBoom : AIAction
{
    private Enemy_Boom enemy;
    [SerializeField]
    private DamageOnTouch boom;

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boom>();
    }

    public override void OnEnterState()
    {
        enemy.Kill();
        boom.DamageCaused = enemy.BoomDamage;
        boom.gameObject.SetActive(true);
        StartCoroutine(_Routine());
        IEnumerator _Routine()
        {
            yield return new WaitForSeconds(0.1f);
            boom.gameObject.SetActive(false);
        }
    }
    public override void PerformAction()
    {

    }
}

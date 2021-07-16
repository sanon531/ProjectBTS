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
        boom.DamageCaused = enemy.Attack * 3;
        boom.gameObject.SetActive(true);
        StartCoroutine(_Routine());
        enemy.Kill();
        IEnumerator _Routine()
        {
            yield return new WaitForSeconds(0.2f);
            boom.GetComponent<Health>().Kill();
        }
    }
    public override void PerformAction()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

public class AIActionBoom : AIAction
{
    private Enemy_Boom enemy;
    [SerializeField]
    private LayerMask layermask;

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boom>();
    }

    public override void OnEnterState()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.BoomRadius, layermask);
        for (int i = 0; i < col.Length; ++i)
        {
            Health health = col[i].GetComponentInParent<Health>();
            if (health != null)
            {
                health.Damage(enemy.Attack * 3, enemy.gameObject, 0, 0, col[i].transform.position - enemy.transform.position);
            }
        }
        enemy.BoomFeedback?.PlayFeedbacks(enemy.transform.position);
        enemy.Kill();
        
    }


    public override void PerformAction()
    {

    }
}

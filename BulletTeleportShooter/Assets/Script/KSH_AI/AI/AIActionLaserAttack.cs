using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using MoreMountains.TopDownEngine;

public class AIActionLaserAttack : AIAction
{
    private float time;
    protected Character _character;
    private Enemy_Boss enemy;

    private List<Sequence> sequenceList;

    [Header("- Area")]
    [SerializeField] private Transform boomArea;
    [SerializeField] private Transform boomReadyArea;
    [SerializeField] private LayerMask layermask;

    protected override void Initialization()
    {
        enemy = GetComponentInParent<Enemy_Boss>();
        _character = GetComponentInParent<Character>();
        sequenceList = new List<Sequence>();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        time = 0f;
        sequenceList.Clear();
    }

    public override void PerformAction()
    {
        if (enemy.CurrentHP > 0)
        {
            if (time <= 0f)
            {
                Vector3 pos = _brain.Target.transform.position;

                Transform newBoomArea = Instantiate(boomArea, pos, Quaternion.identity);
                Transform newBoomReadyArea = newBoomArea.GetChild(0);

                newBoomArea.gameObject.SetActive(true);
                newBoomArea.localScale = new Vector3(enemy.LaserRadius * 2, enemy.LaserRadius * 2, 1);
                newBoomReadyArea.localScale = new Vector3(0, 0, 1);

                Sequence readySequence = DOTween.Sequence();
                readySequence.
                    Append(newBoomReadyArea.DOScaleX(1, enemy.LaserDelay)).
                    Join(newBoomReadyArea.DOScaleY(1, enemy.LaserDelay)).
                    SetEase(Ease.Linear).
                    OnComplete(() =>
                    {
                        Destroy(newBoomArea.gameObject);
                        Destroy(newBoomReadyArea.gameObject);
                        Collider2D[] col = Physics2D.OverlapCircleAll(pos, enemy.LaserRadius, layermask);
                        for (int i = 0; i < col.Length; ++i)
                        {
                            Health health = col[i].GetComponentInParent<Health>();
                            if (health != null)
                            {
                                health.Damage(enemy.LaserAttack, enemy.gameObject, 0, 0, col[i].transform.position - pos);
                            }
                        }
                        enemy.LaserFeedback?.PlayFeedbacks(pos);
                    });
                sequenceList.Add(readySequence);
                time = enemy.LaserDelay;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
        else
        {
            if(sequenceList.Count > 0)
            {
                for(int i = 0; i < sequenceList.Count; ++i)
                {
                    if (sequenceList[i].IsActive())
                    {
                        sequenceList[i].Kill();
                    }
                }
                sequenceList.Clear();
            }
        }
    }
}

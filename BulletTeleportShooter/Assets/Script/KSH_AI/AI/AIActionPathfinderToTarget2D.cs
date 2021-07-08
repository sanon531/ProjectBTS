using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionPathfinderToTarget2D : AIAction
{
    [Tooltip("각 위치에 도달하는 것을 판정하는 최소 거리")]
    public float MinimumDistance = .1f;

    private static float UPDATE_PATH_DELAY = 1f;

    protected Collider2D _collider;
    protected CharacterMovement _characterMovement;
    protected Queue<Vector3> _movePoint;
    protected float _updatePathDelay;

    protected override void Initialization()
    {
        _movePoint = new Queue<Vector3>();
        _collider = this.gameObject.GetComponentInParent<Collider2D>();
        _characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
    }
    public override void PerformAction()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (_brain.Target == null) return;

        _updatePathDelay -= Time.deltaTime;

        if(_updatePathDelay < 0)
        {
            _updatePathDelay = UPDATE_PATH_DELAY;
            _movePoint.Clear();

            Vector2 direction = _brain.Target.position - _collider.bounds.center;
            RaycastHit2D hit = Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, direction.normalized, direction.magnitude, LayerMask.GetMask("Obstacles"));
            if (!hit)
            {
                _movePoint.Enqueue(_brain.Target.position);
            }
            else
            {
                List<Vector3> pathData = PathManager.Instance.FindPath(this.transform.position, _brain.Target.position);

                if (pathData.Count > 1)
                {
                    // 백워킹 방지용 보정
                    float firstPointLength = Vector3.SqrMagnitude(pathData[1] - pathData[0]);
                    float nowPointLength = Vector3.SqrMagnitude(pathData[1] - this.transform.position);
                    if (nowPointLength < firstPointLength)
                    {
                        pathData.RemoveAt(0);
                    }
                }

                for (int i = 0; i < pathData.Count; ++i)
                {
                    _movePoint.Enqueue(pathData[i]);
                }
            }
        }

        if (_movePoint.Count <= 0) return;

        Vector3 destination = _movePoint.Peek();
        int checkStatus = 0;

        if (this.transform.position.x < destination.x)
        {
            _characterMovement.SetHorizontalMovement(1f);
        }
        else
        {
            _characterMovement.SetHorizontalMovement(-1f);
        }

        if (this.transform.position.y < destination.y)
        {
            _characterMovement.SetVerticalMovement(1f);
        }
        else
        {
            _characterMovement.SetVerticalMovement(-1f);
        }

        if (Mathf.Abs(this.transform.position.x - destination.x) < MinimumDistance)
        {
            _characterMovement.SetHorizontalMovement(0f);
            checkStatus += 1;
        }

        if (Mathf.Abs(this.transform.position.y - destination.y) < MinimumDistance)
        {
            _characterMovement.SetVerticalMovement(0f);
            checkStatus += 1;
        }

        if(checkStatus >= 2)
        {
            checkStatus = 0;
            _movePoint.Dequeue();
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
        _characterMovement?.SetHorizontalMovement(0f);
        _characterMovement?.SetVerticalMovement(0f);

        _movePoint.Clear();
        _updatePathDelay = 0;
    }
}

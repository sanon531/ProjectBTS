using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;

public class AIActionPathfinderToTarget2D : AIAction
{
    [Tooltip("각 위치에 도달하는 것을 판정하는 최소 거리")]
    public float MinimumDistance = .1f;

    private static float UPDATE_PATH_DELAY = .5f;

    protected Enemy _enemy;
    protected CircleCollider2D _collider;
    protected CharacterMovement _characterMovement;
    protected Queue<Vector3> _movePoint;
    protected Vector3 _destination;
    protected float _updatePathDelay;

    protected override void Initialization()
    {
        _movePoint = new Queue<Vector3>();
        _enemy = gameObject.GetComponentInParent<Enemy>();
        _collider = (CircleCollider2D)_enemy.Collider;
        _characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
    }
    public override void PerformAction()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        if (PathManager.Instance.IsDebugMode && _movePoint != null && _movePoint.Count > 0)
        {
            Vector3[] points = _movePoint.ToArray();
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, points[0]);
            for (int i = 1; i < points.Length; ++i)
            {
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }
    }

    protected virtual void Move()
    {
        if (_brain.Target == null) return;

        _updatePathDelay -= Time.deltaTime;

        if(_updatePathDelay < 0)
        {
            _updatePathDelay = UPDATE_PATH_DELAY;
            _movePoint.Clear();
            List<Vector3> pathData = PathManager.Instance.FindPath(this.transform.position, _brain.Target.position);
            for (int i = 0; i < pathData.Count; ++i)
            {
                _movePoint.Enqueue(pathData[i]);
                if (i < pathData.Count - 2)
                {
                    Vector2 _direction = _brain.Target.position - pathData[i];
                    RaycastHit2D hit = Physics2D.CircleCast(pathData[i], _collider.radius, _direction.normalized, _direction.magnitude, LayerMask.GetMask("Obstacles"));
                    if (!hit)
                    {
                        _movePoint.Enqueue(_brain.Target.position);
                        break;
                    }
                }
            }
            _destination = _movePoint.Peek();
        }

        _characterMovement.SetMovement((_destination - this.transform.position).normalized);

        int checkStatus = 0;

        if (Mathf.Abs(this.transform.position.x - _destination.x) < MinimumDistance)
        {
            _characterMovement.SetHorizontalMovement(0f);
            checkStatus += 1;
        }

        if (Mathf.Abs(this.transform.position.y - _destination.y) < MinimumDistance)
        {
            _characterMovement.SetVerticalMovement(0f);
            checkStatus += 1;
        }

        if(checkStatus >= 2)
        {
            checkStatus = 0;
            if(_movePoint.Count > 1)
            {
                _movePoint.Dequeue();
                _destination = _movePoint.Peek();
            }
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

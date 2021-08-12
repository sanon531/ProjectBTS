using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class Enemy : MonoBehaviour
{
    public bool IsInit { get; private set; }

    [Header("- Componenets")]
    [SerializeField] protected Health health;
    [SerializeField] protected DamageOnTouch damageOnTouch;
    [SerializeField] protected CharacterMovement movement;
    [SerializeField] protected CharacterRun run;
    [SerializeField] protected Character character;
    [SerializeField] protected new Collider2D collider;
    [SerializeField] protected SpriteOutline spriteOutline;

    [Header("- Default Status")]
    [SerializeField] private int defaultHP;
    [SerializeField] private int defaultAttack;
    [SerializeField] private float defaultSpeed;

    public Action onDeath;
    
    public Collider2D Collider { get => collider; }
    public SpriteOutline Outline { get => spriteOutline; }

    public int DefaultHP
    {
        get
        {
            return defaultHP;
        }
    }

    public int DefaultAttack
    {
        get
        {
            return defaultAttack;
        }
    }

    public float DefaultSpeed
    {
        get
        {
            return defaultSpeed;
        }
    }

    public int CurrentHP
    {
        get
        {
            try
            {
                return health.CurrentHealth;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }
    }
    public int MaxHP
    {
        get
        {
            return health.MaximumHealth;
        }
        set
        {
            try
            {
                float hpRatio = (float)health.CurrentHealth / health.MaximumHealth;
                health.MaximumHealth = value;
                health.SetHealth((int)Mathf.Min(health.MaximumHealth * hpRatio, health.MaximumHealth));
            }
            catch (DivideByZeroException)
            {
                Debug.Log("최대 체력은 0이 될 수 없습니다.");
            }
            catch (NullReferenceException)
            {

            }
        }
    }
    public int Attack
    {
        get
        {
            return damageOnTouch.DamageCaused;
        }
        set
        {
            try
            {
                damageOnTouch.DamageCaused = value;
            }
            catch (NullReferenceException)
            {

            }
        }
    }
    public float Speed
    {
        get
        {
            return movement.MovementSpeed;
        }
        set
        {
            try
            {
                movement.MovementSpeed = value;
            }
            catch (NullReferenceException)
            {

            }
        }
    }
    public virtual Enemy Init()
    {
        if (health == null) health = GetComponent<Health>();
        if (damageOnTouch == null) damageOnTouch = GetComponent<DamageOnTouch>();
        if (movement == null) movement = GetComponent<CharacterMovement>();
        if (run == null) run = GetComponent<CharacterRun>();
        if (character == null) character = GetComponent<Character>();
       
        if(character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            character.RespawnAt(transform, Character.FacingDirections.East);
        }

        MaxHP = defaultHP;
        Attack = defaultAttack;
        Speed = defaultSpeed;

        health.OnDeath += OnDeath;

        IsInit = true;

        return this;
    }

    private void Start()
    {
        if (IsInit == false)
        {
            Init();
        }
    }

    private void OnDeath()
    {
        GameManager.Instance.AddPoints(MaxHP * 10);
        onDeath?.Invoke();
        onDeath = null;
        health.OnDeath -= OnDeath;
    }

    public void Kill()
    {
        health.Kill();
    }

    public Enemy SetActive(bool _state)
    {
        gameObject.SetActive(_state);
        return this;
    }

    public Enemy SetPosition(Vector3 _pos)
    {
        transform.position = _pos;
        return this;
    }
}

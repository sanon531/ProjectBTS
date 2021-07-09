using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class Enemy : MonoBehaviour
{
    public bool IsInit { get; private set; }

    [Header("- Componenets")]
    [SerializeField] protected Health health;
    [SerializeField] protected DamageOnTouch damageOnTouch;
    [SerializeField] protected CharacterMovement movement;
    [SerializeField] protected CharacterRun run;

    [Header("- Default Status")]
    [SerializeField] private int defaultHP;
    [SerializeField] private int defaultAttack;
    [SerializeField] private float defaultSpeed;

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
                health.SetHealth((int)(health.CurrentHealth * hpRatio));
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

        MaxHP = defaultHP;
        Attack = defaultAttack;
        Speed = defaultSpeed;

        IsInit = true;

        return this;
    }

    public void Kill()
    {
        health.Kill();
    }
}

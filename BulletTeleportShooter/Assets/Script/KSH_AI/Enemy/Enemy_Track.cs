using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Track : Enemy
{
    [Header("- Current Status")]
    [SerializeField] private int currentMaxHP;
    [SerializeField] private int currentAttack;
    [SerializeField] private float currentSpeed;

    private void Update()
    {
        currentMaxHP = MaxHP;
        currentAttack = Attack;
        currentSpeed = Speed;
    }
}

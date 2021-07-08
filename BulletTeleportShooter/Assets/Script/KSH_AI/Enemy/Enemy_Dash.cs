using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dash : Enemy
{
    private Timer timer;
    public override Enemy Init()
    {
        timer = FindObjectOfType<Timer>();
        if (timer != null && timer.GameTime > 60f)
        {
            float buffValue = (timer.GameTime - 60f) * .1f;
            this.MaxHP = (int)(this.DefaultHP * (1 + buffValue));
            this.Attack = (int)(this.DefaultAttack *  (1 +buffValue));
            this.Speed = this.DefaultSpeed * (1 + buffValue);
        }
        return base.Init();
    }

    private void Start()
    {
        if (IsInit == false)
        {
            Init();
        }
    }

    private void Update()
    {
        if (timer != null && timer.GameTime > 60f)
        {
            float buffValue = (timer.GameTime - 60f) * .1f;
            this.MaxHP = (int)(this.DefaultHP * (1 + buffValue));
            this.Attack = (int)(this.DefaultAttack * (1 + buffValue));
            this.Speed = this.DefaultSpeed * (1 + buffValue);
        }
    }
}

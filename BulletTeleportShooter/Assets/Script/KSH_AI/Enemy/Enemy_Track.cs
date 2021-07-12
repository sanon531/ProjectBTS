using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Track : Enemy
{
    private Timer timer;
    public override Enemy Init()
    {
        timer = FindObjectOfType<Timer>();
        if (timer != null && timer.GameTime > 10f)
        {
            float buffValue = timer.GameTime * .1f;
            this.MaxHP = (int)(this.DefaultHP * buffValue);
            this.Attack = (int)(this.DefaultAttack * buffValue);
            this.Speed = this.DefaultSpeed * buffValue;
        }
        return base.Init();
    }

    private void Start()
    {
        if(IsInit == false)
        {
            Init();
        }
    }

    private void Update()
    {
        if(timer != null && timer.GameTime > 10f)
        {
            float buffValue = timer.GameTime * .1f;
            this.MaxHP = (int)(this.DefaultHP * buffValue);
            this.Attack = (int)(this.DefaultAttack * buffValue);
            this.Speed = this.DefaultSpeed * buffValue;
        }
    }


}

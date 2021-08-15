using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AnimationScript : MonoBehaviour
{
    [SerializeField]
    private Animator bossAnimator;

    // Start is called before the first frame update
    void Start()
    {
        if (bossAnimator == null)
            bossAnimator = GetComponent<Animator>();        
    }

    private void SetAllFalse()
    {
        bossAnimator.SetBool("IsRushWait", false);
        bossAnimator.SetBool("IsRush", false);
        bossAnimator.SetBool("IsLaserWait", false);
        bossAnimator.SetBool("IsLaserShoot", false);
        bossAnimator.SetBool("IsWalk", false);
        bossAnimator.SetBool("IsAlive", true);

    }

    public void StartWalk()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsWalk", true);
    }
    public void StartRushWait()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsRushWait", true);
    }
    public void StartRush()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsRush", true);
    }
    public void StartLaserWait()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsLaserWait", true);
    }
    public void StartLaserShoot()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsLaserShoot", true);
    }

    public void Die()
    {
        SetAllFalse();
        bossAnimator.SetBool("IsAlive", false);
    }

    //임시 코드 실험용

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
            StartWalk();

        if (Input.GetKeyDown(KeyCode.S))
            StartRush();

        if (Input.GetKeyDown(KeyCode.Z))
            StartLaserWait();

        if (Input.GetKeyDown(KeyCode.X))
            StartLaserShoot();

        if (Input.GetKeyDown(KeyCode.C))
            Die();



    }






}

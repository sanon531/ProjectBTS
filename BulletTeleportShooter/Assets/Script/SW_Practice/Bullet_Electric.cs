using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Electric : MonoBehaviour
{
    [SerializeField]
    private Laser_FX_Continue thisElectric;

    private void Reset()
    {
        thisElectric = GetComponent<Laser_FX_Continue>();
    }

    // Start is called before the first frame update
    void Start()
    {
        thisElectric = GetComponent<Laser_FX_Continue>();
    }

    private void OnEnable()
    {
        //thisElectric.SetTransform();
    }


    private void OnDisable()
    {
        //thisElectric.ResetTransform();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

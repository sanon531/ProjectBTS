using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthMapData : MonoBehaviour
{
    public Animator animator;
    public int number = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (number == 1)
        {
            FifthMapSpikeContainer.FirstContainer.AddSpike(this);
        }
        else
        {
            FifthMapSpikeContainer.SecondContainer.AddSpike(this);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

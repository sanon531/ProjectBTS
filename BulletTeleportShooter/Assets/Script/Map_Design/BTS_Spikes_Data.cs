using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTS_Spikes_Data : MonoBehaviour
{

   public Animator animator;


    // Start is called before the first frame update
    private void Start()
    {
        SpikeObjectManager.Instance.AddSpike(this);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

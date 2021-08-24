using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeObjectManager : MonoBehaviour
{

    /*[SerializeField]
    List<BTS_Object_Data> OnOffBlockList = new List<BTS_Object_Data>();
    */
    [SerializeField]
    List<BTS_Spikes_Data> SpikeList = new List<BTS_Spikes_Data>();

    public static SpikeObjectManager Instance;
    public float fasttimer;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    /*
    public void AddOnOff(BTS_Object_Data data)
    {
        OnOffBlockList.Add(data);
    }
    */
    public void AddSpike(BTS_Spikes_Data data)
    {
        SpikeList.Add(data);
    }

    public void OnClickSpike_Reset()
    {
        spike_speed = 1;
        for (int i = 0; i < SpikeList.Count; i++)
        {
            SpikeList[i].animator.SetFloat("spikeupd", 1);
        }

    }

    public void SpikeStop()
    {
        if (IsPuase)
        {
            for (int i = 0; i < SpikeList.Count; i += 2)
            {
                SpikeList[i].animator.speed = 0;
            }
        }
        else 
            {
                for (int i = 0; i < SpikeList.Count; i += 2)
                {
                    SpikeList[i].animator.speed = 1;
                }
            }
    }

    private bool IsPuase = true;
    public float spike_timer = 0f;
    public float spike_speed = 1f;
    

    // Update is called once per frame
    void Update()
    {
        
        if (spike_timer >=1.5f)
        {
            IsPuase = false;
        }
        SpikeStop();
        
        spike_timer += Time.deltaTime;
        
        if (spike_timer > fasttimer)
        {

            spike_speed += 0.5f;
            for (int i = 0; i < SpikeList.Count; i++)
            {
                SpikeList[i].animator.SetFloat("spikeupd", spike_speed);
            }
            spike_timer = 0f;
        }
        
       
    }
}

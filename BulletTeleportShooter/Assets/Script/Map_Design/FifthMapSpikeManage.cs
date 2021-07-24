using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthMapSpikeManage : MonoBehaviour
{
    public static FifthMapSpikeManage Instance;

    [SerializeField]
    List<FifthMapSpikeContainer> SpikeContainerList = new List<FifthMapSpikeContainer>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void Spike_Stop()
    {
        if (IsPuase)
        {
            FifthMapSpikeContainer.SecondContainer.Change_Speed_z();
        }
        else
        {
            FifthMapSpikeContainer.SecondContainer.Change_Speed_o();
        }
    }

    private bool IsPuase = true;

    public float spike_timer = 0f;
    public float spike_speed = 1f;
    public int c = 0;

    public float t_1 = 0;
    public float t_2 = 1.5f;

    public int count_1 = 0;
    public int count_2 = 0;

    // Update is called once per frame
    void Update()
    {
        
        spike_timer += Time.deltaTime;
        t_1 += Time.deltaTime;
        t_2 += Time.deltaTime;
        if (spike_timer >2.5f && c == 0)
        {
            c++;
            IsPuase = false;
        }
        Spike_Stop();
        
        if (spike_timer > 30f)
        { 
            spike_speed++;
            for (int i = 0; i < FifthMapSpikeContainer.FirstContainer.Counting(); i++)
            {
                FifthMapSpikeContainer.FirstContainer.Change_SpikeSpeed(spike_speed);
                FifthMapSpikeContainer.SecondContainer.Change_SpikeSpeed(spike_speed);

            }
            spike_timer = 0f;
        }


        
    }

}

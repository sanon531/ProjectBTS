using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthMapSpikeManage : MonoBehaviour
{
    public static FifthMapSpikeManage Instance;


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
    public float spike_speed = 0f;
    public int c = 0;
    public int s = 0;

    public float t_1 = 0;
    public float t_2 = 0;

    public int count_1 = 0;
    public int count_2 = 0;

    // Update is called once per frame
    void Update()
    {
        
        spike_timer += Time.deltaTime;
        t_1 += Time.deltaTime;
        t_2 += Time.deltaTime;
        /*
        if (spike_timer >2.5f && c == 0)
        {
            c++;
            IsPuase = false;
        }
        Spike_Stop();
        */

        if (c == 0 && t_1 > 0)
        {
            count_1++;
            c++;
            FifthMapSpikeContainer.FirstContainer.Plus_Controller(count_1);
        }
        else if (c == 1 && t_1 > 0.5f)
        {
            count_1++;
            c++;
            FifthMapSpikeContainer.FirstContainer.Plus_Controller(count_1);
        }
        else if (c == 2&& t_1 > 1.5f)
        {
            count_1--;
            c++;
            FifthMapSpikeContainer.FirstContainer.Minus_Controller(count_1);
        }
        else if (c==3&&t_1 > 2f)
        {
            count_1--;
            c++;
            FifthMapSpikeContainer.FirstContainer.Minus_Controller(count_1);
        }
       
        

        if (s == 0 && t_2 > 2f)
        {
            count_2++;
            s++;
            FifthMapSpikeContainer.SecondContainer.Plus_Controller(count_2);
        }
        else if (s == 1 && t_2 > 2.5f)
        {
            count_2++;
            s++;
            FifthMapSpikeContainer.SecondContainer.Plus_Controller(count_2);
        }
        else if (s == 2&& t_2 > 3.5f)
        {
            count_2--;
            s++;
            FifthMapSpikeContainer.SecondContainer.Minus_Controller(count_2);

        }
        else if (s == 3 && t_2 > 4f)
        {
            count_2--;
            s++;
            FifthMapSpikeContainer.SecondContainer.Minus_Controller(count_2);
        }
        else if (s == 4 && t_2 > 6f)
        {
            s = 0;
            t_2 = 0;
            t_1 = 0;
            c = 0;
        }



        if (spike_timer > 3f)
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

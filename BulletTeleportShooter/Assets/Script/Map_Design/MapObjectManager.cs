using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectManager : MonoBehaviour
{

    [SerializeField]
    List <BTS_Object_Data> OnOffBlockList = new List <BTS_Object_Data> ();
    List<BTS_Spikes_Data> SpikeList = new List<BTS_Spikes_Data>();

    public static MapObjectManager Instance;
    public Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddOnOff(BTS_Object_Data data)
    {
        OnOffBlockList.Add(data);
    }

    public void AddSpike(BTS_Spikes_Data data)
    {
        SpikeList.Add(data);
    }


    public float spike_timer = 0f;
    public float spike_speed = 1f;

    //float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        spike_timer += Time.deltaTime;
        
        if (spike_timer > 30f)
        {

            spike_speed++;
            for (int i = 0; i < SpikeList.Count; i++)
            {
                SpikeList[i].animator.SetFloat("spikeupd", spike_speed);
            }
            spike_timer = 0f;
        }
        
        

        //Timer Timer_GameTime = GameObject.Find("GameManager").GetComponent<Timer>();
        //timer = Timer_GameTime.GameTime;

        //timer += Time.deltaTime;

        /*
        if (timer > 5f)
        {
            for (int i = 0; i<OnOffBlockList.Count; i++)
            {
                OnOffBlockList[i].OnOff();
            }
            timer = 0f;
        }
        */
    }
}

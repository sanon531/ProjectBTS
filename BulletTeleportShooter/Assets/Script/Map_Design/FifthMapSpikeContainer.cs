using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FifthMapSpikeContainer : MonoBehaviour
{

    public static FifthMapSpikeContainer FirstContainer;
    public static FifthMapSpikeContainer SecondContainer;

    public Animator animator;

    public int number = 1;

    [SerializeField]
    List<FifthMapData> Spike_List = new List<FifthMapData>();

    public void AddSpike(FifthMapData data)
    {
        Spike_List.Add(data);
    }

    public void Change_Speed_z()
    {
        for (int i = 0; i < Spike_List.Count; i++)
        {
            Spike_List[i].animator.speed = 0;
        }
    }
    public void Change_Speed_o()
    {
        for (int i = 0; i < Spike_List.Count; i++)
        {
            Spike_List[i].animator.speed = 1;
        }
    }

    public void Change_SpikeSpeed(float spike_speed)
    {
        for (int i = 0; i < Spike_List.Count; i++)
        {
            Spike_List[i].animator.SetFloat("spikeupd", spike_speed);
        }
    }

    void Awake()
    {
        if (number == 1)
        {
            FirstContainer = this;
        }
        else
        {
            SecondContainer = this;
        }
    }

    public int Counting()
    {
        return Spike_List.Count;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectContainer : MonoBehaviour
{
    public static BrokenObjectContainer First_Container;
    public static BrokenObjectContainer Second_Container;
    public static BrokenObjectContainer Third_Container;

    public int number = 1;

    [SerializeField]
    List<BrokenObjectData> BrokenList = new List <BrokenObjectData>();
    public ParticleSystem BrokenFX;

    public void AddBrokenEffect(BrokenObjectData data)
    {
        BrokenList.Add(data);
    }


    void Awake()
    {
        if (number == 1)
        {
            First_Container = this;
        }
        else if (number == 2)
        {
            Second_Container = this;
        }
        else
        {
            Third_Container = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

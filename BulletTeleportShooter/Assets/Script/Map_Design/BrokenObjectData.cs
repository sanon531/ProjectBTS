using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectData : MonoBehaviour
{
    public ParticleSystem BrokenFX;

    
    public int number = 0;

    // Start is called before the first frame update
    private void Start()
    {
        if (number == 0)
        {
            BrokenObjectContainer.First_Container.AddBrokenEffect(this);
        }
        else if (number == 1)
        {
            BrokenObjectContainer.Second_Container.AddBrokenEffect(this);
        }
        else
        {
            BrokenObjectContainer.Third_Container.AddBrokenEffect(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTS_HoleObjectData : MonoBehaviour
{
    public int number= 0;

    // Start is called before the first frame update
    void Start()
    {

        if (number == 0)
        {
            BTS_HoleObjectContainer.FirstContainer.AddHole(this);
        }
        else if (number == 1)
        {
            BTS_HoleObjectContainer.SecondContainer.AddHole(this);
        }
        else
        {
            BTS_HoleObjectContainer.ThirdContainer.AddHole(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

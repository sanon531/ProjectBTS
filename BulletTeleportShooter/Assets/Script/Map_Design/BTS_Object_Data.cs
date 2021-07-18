using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;

public class BTS_Object_Data : MonoBehaviour
{

    public BoxCollider2D _boxColider2D;
    
    // Start is called before the first frame update
    private void Start()
    {
        SpikeObjectManager.Instance.AddOnOff(this);
        _boxColider2D = GetComponent<BoxCollider2D>();
    }

    bool turnOn = true;

    public void OnOff()
    {
        if (turnOn)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            _boxColider2D.enabled = false;
            turnOn = false;
            gameObject.GetComponent<DamageOnTouch>().enabled = false;
        }

    
        else
        {
    
            GetComponent<SpriteRenderer>().enabled = true;
            _boxColider2D.enabled = true;
            turnOn = true;
            gameObject.GetComponent<DamageOnTouch>().enabled = true;

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

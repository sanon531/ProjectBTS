using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{


    public float moveTime = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * moveTime * Time.deltaTime);
        
    }
}

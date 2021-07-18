using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSDestroyTile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float t = 0;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t>=5f)
        {
            Destroy(gameObject);
        }
    }
}

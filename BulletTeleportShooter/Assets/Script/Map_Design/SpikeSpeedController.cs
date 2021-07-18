using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpeedController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    private float t = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        Debug.Log(t);

        if (t>=1.5f)
        {
            Time.timeScale = 1;
        }
    }
}

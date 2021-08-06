using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObjectData : MonoBehaviour
{
    public ParticleSystem BrokenFX;

    public void Play_Broken()
    {
        BrokenFX.Play();
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

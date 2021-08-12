using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTS_DustEffect : MonoBehaviour
{
    public ParticleSystem DustFX;
    public float t;

    Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DustStart(t));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DustStart(float time)
    {
        yield return new WaitForSeconds(time-5);
        DustFX.Play();
    }
}

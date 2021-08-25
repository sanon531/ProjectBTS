using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITweener : MonoBehaviour
{
    public RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cococo());
    }

    IEnumerator cococo()
    {
        yield return new WaitForEndOfFrame();
        


    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

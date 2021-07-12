using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class practicejoon : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(10, 10, 2), 2, false).SetEase(Ease.InQuad);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

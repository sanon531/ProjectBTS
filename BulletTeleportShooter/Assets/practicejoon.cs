using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class practicejoon : MonoBehaviour
{
    public int num = 100;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(Vector3.up, 50);
        transform.DOScale(Vector3.one * 3, 50);
        transform.DORotate(Vector3.forward * num, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

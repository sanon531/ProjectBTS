using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Credit : MonoBehaviour
{
    public RectTransform MainMenu;
    [SerializeField]
    private float time = 0.5f;
    [SerializeField]
    private float XPos = 2000;
    [SerializeField]
    private Vector3 targetPos;


    public void Start()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().onClick.AddListener(() => OnClick());
    }
    public void OnClick()
    {
        MainMenu.DOAnchorPos(targetPos,time);
    }


    public void OnCredit()
    {
        MainMenu.DOAnchorPosX(XPos, time);
       
    }
}

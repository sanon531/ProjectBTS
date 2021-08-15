using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Credit : MonoBehaviour
{
    public RectTransform MainMenu;
    public float time = 0.5f;
    
    
    
    public void OnCredit()
    {
        MainMenu.DOAnchorPosX(2000,time);
       
    }
}

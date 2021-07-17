using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_Char : MonoBehaviour
{
    Sequence animSequence;


    public RectTransform rectTransform;
    public Vector2 OriginPos;
    public Image image;
   // public Tween tween;


    private void Start()
    {
        //image.DOFade(0f, 0f);
        OriginPos = rectTransform.anchoredPosition;
        Debug.Log(OriginPos);
    }
    



    public void OnClick()
    {
       

        rectTransform.DOScale(2f,1); //scale을 2로
        
        //rectTransform.DOAnchorPos(new Vector2(0, 0), 1, false);    // (0,0)으로 이동
        
        if (rectTransform.anchoredPosition.x > -20)
        {
           
            rectTransform.DOAnchorPos(new Vector2(1280, 0), 1, false);    // 기존 위치(에서 +640)로 이동
        }

        if (-700 < rectTransform.anchoredPosition.x && rectTransform.anchoredPosition.x < -20)
        {
            
            rectTransform.DOAnchorPos(new Vector2(0,0), 1, false);    // 기존 위치로 이동
        }

        if (rectTransform.anchoredPosition.x < -700)
        {
            
            rectTransform.DOAnchorPos(new Vector2(-1280, 0), 1, false);    // 기존 위치(에서 -640)로 이동
        }
        //tween = image.DOFade(1f, 1f);
    }

    

    public void OffClick() //되돌리기
    {
        rectTransform.DOScale(1f, 1); //1배 1초

        if (rectTransform.anchoredPosition.x >= 1)
        {
            
            rectTransform.DOAnchorPos(new Vector2(OriginPos.x + 640,OriginPos.y), 1, false);    // 기존 위치(에서 +640)로 이동
        }

        if (-1< rectTransform.anchoredPosition.x && rectTransform.anchoredPosition.x < 1)
        {
            
            rectTransform.DOAnchorPos(new Vector2(OriginPos.x + 1,OriginPos.y + 1), 1, false);    // 기존 위치로 이동
        }
        
        if (rectTransform.anchoredPosition.x <= -1)
        {
            
            rectTransform.DOAnchorPos(new Vector2(OriginPos.x - 640, OriginPos.y), 1, false);    // 기존 위치(에서 -640)로 이동
        }

        //tween = image.DOFade(0f, 1f);

    }
}

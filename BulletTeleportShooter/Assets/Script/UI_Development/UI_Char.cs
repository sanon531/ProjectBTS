using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_Char : MonoBehaviour
{
    //폐기


    public RectTransform rect;
    public Vector2 OriginPos;
    //public Image image;
   // public Tween tween;


    private void Start()
    {
        //image.DOFade(0f, 0f);
        OriginPos = rect.anchoredPosition;
        Debug.Log(OriginPos);
    }
    



    public void OnClick()
    {
       

        rect.DOScale(2f,1); //scale을 2로
        
        //rectTransform.DOAnchorPos(new Vector2(0, 0), 1, false);    // (0,0)으로 이동
        
        if (rect.anchoredPosition.x > -20)
        {

            rect.DOAnchorPos(new Vector2(1280, 0), 1, false);    // 기존 위치(에서 +640)로 이동
        }

        if (-700 < rect.anchoredPosition.x && rect.anchoredPosition.x < -20)
        {

            rect.DOAnchorPos(new Vector2(0,0), 1, false);    // 기존 위치로 이동
        }

        if (rect.anchoredPosition.x < -700)
        {

            rect.DOAnchorPos(new Vector2(-1280, 0), 1, false);    // 기존 위치(에서 -640)로 이동
        }
        //tween = image.DOFade(1f, 1f);
    }

    

    public void OffClick() //되돌리기
    {
        rect.DOScale(1f, 1); //1배 1초

        if (rect.anchoredPosition.x >= 1)
        {

            rect.DOAnchorPos(new Vector2(OriginPos.x + 640,OriginPos.y), 1, false);    // 기존 위치(에서 +640)로 이동
        }

        if (-1< rect.anchoredPosition.x && rect.anchoredPosition.x < 1)
        {

            rect.DOAnchorPos(new Vector2(OriginPos.x + 1,OriginPos.y + 1), 1, false);    // 기존 위치로 이동
        }
        
        if (rect.anchoredPosition.x <= -1)
        {

            rect.DOAnchorPos(new Vector2(OriginPos.x - 640, OriginPos.y), 1, false);    // 기존 위치(에서 -640)로 이동
        }

        //tween = image.DOFade(0f, 1f);

    }
}


// 1번 아예 안눌리게 한다 2번 조건 범
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
    public Tween tween;


    private void Start()
    {
        image.DOFade(0f, 0f);
        OriginPos = rectTransform.anchoredPosition;
        Debug.Log(OriginPos);
    }




    public void OnClick()
    { 
      
        rectTransform.DOScale(1.4f,1); //1.4배 1초
        rectTransform.DOAnchorPos(new Vector2(0, 0), 1, false);    // (0,0)으로 이동
        tween = image.DOFade(1f, 1f);
    }

    

    public void OffClick() //되돌리기
    {
        rectTransform.DOScale(0.5f, 1); //0.5배 1초
        rectTransform.DOAnchorPos(OriginPos, 1, false);    // (-730,-403)으로 이동
        tween = image.DOFade(0f, 1f);

    }
}

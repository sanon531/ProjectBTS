using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_Char : MonoBehaviour
{
    Sequence animSequence;


    public RectTransform rectTransform;

    public void OnClick()
    {
        rectTransform.DOScale(1,1);
        rectTransform.DOAnchorPos(new Vector2(0, 0), 1, false);    // (0,0)으로 이동
        
    }
}

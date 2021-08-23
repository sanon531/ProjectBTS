using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;

public class UI_Title : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI text_Title;
    Sequence titleSequence;
    Vector2 defaultPos;

    private void Awake()
    {
        defaultPos = rectTransform.position;
        canvasGroup.alpha = 0;
    }

    public void MakeTitle(string _content, float _time)
    {
        if (titleSequence.IsActive())
        {
            titleSequence.Kill();
        }
        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.rect.height);
        text_Title.SetText(_content);

        titleSequence = DOTween.Sequence();
        titleSequence.
            Append(rectTransform.DOMoveY(defaultPos.y, 1f)).
            AppendInterval(_time).
            Append(rectTransform.DOMoveY(defaultPos.y + rectTransform.rect.height, 1f)).
            AppendCallback(() => canvasGroup.alpha = 0);
    }
    public void MakeTitle(string _content, string _Item, float _time)
    {
        if (titleSequence.IsActive())
        {
            titleSequence.Kill();
        }
        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.rect.height);
        text_Title.SetText(_content);

        titleSequence = DOTween.Sequence();
        titleSequence.
            Append(rectTransform.DOMoveY(defaultPos.y, 1f)).
            AppendInterval(_time).
            Append(rectTransform.DOMoveY(defaultPos.y + rectTransform.rect.height, 1f)).
            AppendCallback(() => canvasGroup.alpha = 0);
    }
}

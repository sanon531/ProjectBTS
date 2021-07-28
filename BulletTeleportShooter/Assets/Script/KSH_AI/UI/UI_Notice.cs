using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_Notice : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI text_Notice;
    Sequence noticeSequence;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public void MakeNotice(string _content, float _time)
    {
        if (noticeSequence.IsActive())
        {
            noticeSequence.Kill();
        }
        canvasGroup.alpha = 0;
        rectTransform.localScale = new Vector3(0, rectTransform.localScale.y, rectTransform.localScale.z);
        text_Notice.SetText(_content);

        noticeSequence = DOTween.Sequence();
        noticeSequence.
            Append(canvasGroup.DOFade(1, 1f)).
            Join(rectTransform.DOScaleX(1, 1f)).
            AppendInterval(_time).
            Append(canvasGroup.DOFade(0, 1f)).
            Join(rectTransform.DOScaleX(0, 1f));
    }
}

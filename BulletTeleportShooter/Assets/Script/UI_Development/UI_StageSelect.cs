using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_StageSelect : MonoBehaviour
{
    public float speed;
    public float scaleVar;
    public float distance;

    public RectTransform rectTransform;
    public RectTransform[] node;
    public int index = 2;

    Sequence animSequence;

    public void BuildAnimation(int _index)
    {
        if (0 <= _index && _index < node.Length)
        {
            animSequence = DOTween.Sequence();
            animSequence.
                Append(rectTransform.DOMoveX(distance * (index > _index ? 1 : -1), speed).SetRelative(true));
            if (0 <= _index - 1 && _index - 1 < node.Length) animSequence.
                    Join(node[_index - 1].DOScale(Vector3.one, speed)); // 왼쪽의 노드 애니메이션 들어갈 부분
            if (0 <= _index && _index < node.Length) animSequence.
                    Join(node[_index].DOScale(Vector3.one * scaleVar, speed)); // 중앙의 노드 애니메이션 들어갈 부분
            if (0 <= _index + 1 && _index + 1 < node.Length) animSequence.
                    Join(node[_index + 1].DOScale(Vector3.one, speed)); // 오른쪽의 노드 애니메이션 들어갈 부분
            animSequence.OnComplete(() => index = _index);
        }
    }

    public void Focus(int _index)
    {
        for(int i = 0; i < node.Length; ++i)
        {
            Debug.Log(node[i].anchoredPosition);
        }

        if (0 <= _index && _index < node.Length)
        {
            node[_index].localScale = Vector3.one * 1.2f;
            rectTransform.anchoredPosition = new Vector2(-node[_index].anchoredPosition.x, node[_index].anchoredPosition.y);
        }
    }

    private void Start()
    {
        Focus(index);
    }

    public void OnClick_Left()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(index - 1);
        }
    }

    public void OnClick_Right()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(index + 1);
        }
    }



}

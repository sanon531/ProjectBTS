using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class UI_CharSelect : MonoBehaviour
{
    public float time = 0.5f;
    public float scaleVar;
    public float distance;

    public RectTransform rectTransform;
    public RectTransform[] node;
    public int uiTargetedIndex = 1;


    Sequence animSequence;

    public void BuildAnimation(int _index)
    {
        if (0 <= _index && _index < node.Length)
        {

            animSequence = DOTween.Sequence();
            animSequence.
                Append(
                rectTransform.DOAnchorPosX(distance * (uiTargetedIndex > _index ? 1 : -1), time).SetRelative(true));
            if (0 <= _index - 1 && _index - 1 < node.Length) animSequence.
                    Join(node[_index - 1].DOScale(Vector3.one, time)); // 왼쪽의 노드 애니메이션 들어갈 부분
            if (0 <= _index && _index < node.Length) animSequence.
                    Join(node[_index].DOScale(Vector3.one * scaleVar, time)); // 중앙의 노드 애니메이션 들어갈 부분
            if (0 <= _index + 1 && _index + 1 < node.Length) animSequence.
                    Join(node[_index + 1].DOScale(Vector3.one, time)); // 오른쪽의 노드 애니메이션 들어갈 부분
            animSequence.OnComplete(() => uiTargetedIndex = _index);
        }
    }

    /*public void Focus(int _index)
    {
        for (int i = 0; i < node.Length; ++i)
        {
            //Debug.Log(node[i].anchoredPosition);
        }

        if (0 <= _index && _index < node.Length)
        {
            node[_index].localScale = Vector3.one * scaleVar;
            rectTransform.anchoredPosition = new Vector2(0, 0); //node[_index].anchoredPosition
        }
    }

    */
    public void OnClick_SelectBack()
    {
        if (uiTargetedIndex == 1)
        {
            node[uiTargetedIndex + 1].gameObject.SetActive(false);
            node[uiTargetedIndex - 1].gameObject.SetActive(false);
        }

        if (uiTargetedIndex == 0)
        {
            node[uiTargetedIndex + 1].gameObject.SetActive(false);
            node[uiTargetedIndex + 2].gameObject.SetActive(false);
        }
        
        if (uiTargetedIndex == 2)
        {
            node[uiTargetedIndex - 2].gameObject.SetActive(false);
            node[uiTargetedIndex - 1].gameObject.SetActive(false);
        }

    }

    public void OnClick_Select()
    {
        if (uiTargetedIndex == 1)
        {
            node[uiTargetedIndex + 1].gameObject.SetActive(true);
            node[uiTargetedIndex - 1].gameObject.SetActive(true);
        }

        if (uiTargetedIndex == 0)
        {
            node[uiTargetedIndex + 1].gameObject.SetActive(true);
            node[uiTargetedIndex + 2].gameObject.SetActive(true);
        }

        if (uiTargetedIndex == 2)
        {
            node[uiTargetedIndex - 2].gameObject.SetActive(true);
            node[uiTargetedIndex - 1].gameObject.SetActive(true);
        }
    }

    public void OnClick_Left()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(uiTargetedIndex - 1);
        }
    }

    public void OnClick_Right()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(uiTargetedIndex + 1);
        }
    }


}

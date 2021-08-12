using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.TopDownEngine;

public class UI_StageSelect : MonoBehaviour
{
    public float time = 0.5f;
    public float scaleVar;
    public float distance;

    public RectTransform rectTransform;
    public RectTransform[] node;
    public int uiTargetedIndex = 0;
    public int ImmediateIndex = 0;
    public List<string> SceneList;
    
    
    public int gold = 0;
    public GameObject selectButton;
    public int[] limitGold;


    
    
    Sequence animSequence;

    public void BuildAnimation(int _index)
    {
        if (0 <= _index && _index < node.Length)
        {

            selectButton.SetActive(true);
            ImmediateIndex = _index;
            LimitedSelect();
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

    public void LimitedSelect()
    {
        for(int i = 0; i<limitGold.Length; i++)
        {
            if(ImmediateIndex == i && gold < limitGold[i])
            {
                selectButton.SetActive(false);
                Debug.Log("골드가 부족합니다.");
                Debug.Log(ImmediateIndex);
                Debug.Log(limitGold);

            }
        }           
            
    }

    public void UnlimitedSelect()
    {
        for (int i = 0; i < limitGold.Length; i++)
        {
            if (uiTargetedIndex == i && gold >= limitGold[i])
            {
                selectButton.SetActive(true);
                Debug.Log(uiTargetedIndex);

                Debug.Log("해금되었습니다.");
                
                Debug.Log(gold);

            }
        }

    }




    public void Focus(int _index)
    {       
        if (0 <= _index && _index < node.Length)
        {
            node[_index].localScale = Vector3.one * scaleVar;
            rectTransform.anchoredPosition = new Vector2(-node[_index].anchoredPosition.x, rectTransform.anchoredPosition.y); //node[_index].anchoredPosition
        }
    }

    private void Start()
    {
        Focus(uiTargetedIndex);
        UnlimitedSelect();
        
    }

   /* private void Update()
    {
        for (int i = 0; i < limit.Length; i++)
        {
            if (ImmediateIndex == i && gold < limit[i])
            {
                Button btn = selectButton.GetComponent<Button>();
                btn.enabled = false;
            }
        }
        
        Button btn1 = selectButton.GetComponent<Button>();
        btn1.enabled = true;
    }
   */
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

    
    
    public void OnClick_Stn()
    {
        if(!animSequence.IsActive())
        {                      
            SceneLoad();
        }
    }          
        
    public void SceneLoad()
    {
        SceneManager.LoadScene(SceneList[ImmediateIndex]);
        DontDestroyOnLoad(GameManager.Instance);
    }



}

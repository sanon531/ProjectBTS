using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoreMountains.TopDownEngine;
using System.IO;
using System;

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
    public SaveData save;


    [SerializeField]
    private bool isMoblie = false;
    public string[] mapName;
    public GameObject[] images;
    public GameObject selectButton;
    //public SaveData saveData = new SaveData();


    public void MapLocker()
    {
        save = SaveAndLoad.instance.saveData;
        uiTargetedIndex = save.lastPlayedMaps;
        ImmediateIndex = uiTargetedIndex;
        //Debug.Log("123654..");

        foreach (KeyValuePair<string, bool> keyValuePair in save.mapLock)
        {
            //Debug.Log("123654");

            for (int i = 0; i < mapName.Length; i++)
            {

                if (keyValuePair.Key == mapName[i])
                {
                    if (keyValuePair.Value)
                    {
                        images[i].SetActive(false);

                    }
                    else
                    {
                        images[i].SetActive(true);
                    }
                }
            }
        }
    }


    public void ButtonLocker()
    {

        for (int i = 0; i < images.Length; i++)
        {
            if (ImmediateIndex == i)
            {
                if (images[i].activeSelf == true)
                {
                    selectButton.GetComponent<Button>().enabled = false;
                    Debug.Log("잠김");
                }
                else
                {
                    selectButton.GetComponent<Button>().enabled = true;
                    Debug.Log("열림");
                }
            }


        }
    }

    Sequence animSequence;

    public void BuildAnimation(int _index)
    {
        if (0 <= _index && _index < node.Length)
        {

            ImmediateIndex = _index;

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
            SaveAndLoad.instance.SetLastMap(_index);

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
        StartCoroutine(coco());
    }

    IEnumerator coco()
    {
        yield return new WaitForEndOfFrame();
        MapLocker();
        Focus(uiTargetedIndex);

    }



    public void OnClick_Left()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(uiTargetedIndex - 1);

            /*Button btn = selectButton.GetComponent<Button>();
            
            btn.enabled = false;
            
            Invoke("OnInvoke", time + 0.1f);
            */
        }
    }

    public void OnClick_Right()
    {
        if (!animSequence.IsActive())
        {
            BuildAnimation(uiTargetedIndex + 1);

            /*Button btn = selectButton.GetComponent<Button>();
            
            btn.enabled = false;

            Invoke("OnInvoke", time + 0.1f);
            */
        }
    }

    /*void OnInvoke()
    {
        LimitedSelect();
        UnlimitedSelect();
    }
    */

    public void OnClick_Stn()
    {
        if (!animSequence.IsActive())
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CutSceneController : MonoBehaviour
{



    //rect transform Data와 연결 시켜서 리스트로 만든뒤 하나하나
    public Transform CutSceneTransform;
    public List<CutSceneData> cutSceneDatas = new List<CutSceneData>();
    public static CutSceneController Instance; 

    private void Awake()
    {
        Instance = this;
    }



    private void Update()
    {
        if (Input.anyKeyDown)
            CallCutScene();
    }
    public float ShowTime = 3f;
    // 리스트 만든 다음 씬을 하나하나 연결 시키고 
    public void CallCutScene()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!cutSceneDatas[0].isActive)
            {
                cutSceneDatas[0].isActive = true;
                cutSceneDatas[0].rectTransform.DOAnchorPos(cutSceneDatas[0].ShowPos, ShowTime).
                SetEase(Ease.OutBack);
            }
            else
            {
                cutSceneDatas[0].isActive = false;
                cutSceneDatas[0].rectTransform.DOAnchorPos(cutSceneDatas[0].HidePos, ShowTime).
                SetEase(Ease.OutBack);

            }

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!cutSceneDatas[0].isActive)
            {
                cutSceneDatas[1].isActive = true;
                cutSceneDatas[1].rectTransform.DOAnchorPos(cutSceneDatas[1].ShowPos, ShowTime).
                SetEase(Ease.OutBack);
            }
            else
            {
                cutSceneDatas[1].isActive = true;
                cutSceneDatas[1].rectTransform.DOAnchorPos(cutSceneDatas[1].ShowPos, ShowTime).
                SetEase(Ease.OutBack);

            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            cutSceneDatas[2].isActive = true;
            cutSceneDatas[2].rectTransform.DOAnchorPos(cutSceneDatas[2].ShowPos, ShowTime).
                OnComplete(() => cutSceneDatas[2].isActive = false).
                SetEase(Ease.OutBack);

        }


    }


    List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }


}

[System.Serializable]
public class CutSceneData
{
    public bool isActive= false;
    public RectTransform rectTransform;
    public Vector2 HidePos;
    public Vector2 ShowPos;


}

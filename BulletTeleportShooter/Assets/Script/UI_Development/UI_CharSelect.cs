using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using System.IO;
using System;


public class UI_CharSelect : MonoBehaviour
{
    public float time = 0.5f;
    //public float scaleVar;
    public float distance1,distance2;

    public RectTransform rectTransform;
    public RectTransform[] node;
    public int uiTargetedIndex = 1;
    public int ImmediateIndex = 0;
    public GameObject Stn, Selectbtn;
    public GameObject[] images;
    public SaveData saveData = new SaveData();
    public string[] gunName;
    public SaveAndLoad save;

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";


    public void GunLocker()
    {

        GunLock gunLock = saveData.gunLock;
        foreach (var keyValuePair in gunLock)
        {

            for (int i = 0; i < gunName.Length; i++)
            {

                if (keyValuePair.Key == gunName[i])
                {
                    if (keyValuePair.Value)
                    {
                        images[i].SetActive(false);
                        Debug.Log("김준");

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
                    Stn.GetComponent<Button>().enabled = false;
                    Debug.Log("잠김");
                }
                else
                {
                    Stn.GetComponent<Button>().enabled = true;
                    Debug.Log("열림");
                }
            }


        }
    }
    Sequence animSequence;

    public void BuildAnimation(int _index) //캐릭터창 좌우 이동
    {
        if (0 <= _index && _index < node.Length)
        {
            ImmediateIndex = _index;
            animSequence = DOTween.Sequence();
            animSequence.
                Append(
                rectTransform.DOAnchorPosX(distance1 * (uiTargetedIndex > _index ? 1 : -1), time).SetRelative(true));
            if (0 <= _index - 1 && _index - 1 < node.Length) animSequence.
                    Join(node[_index - 1].DOScale(Vector3.one, time)); // 왼쪽의 노드 애니메이션 들어갈 부분
            if (0 <= _index && _index < node.Length) animSequence.
                    Join(node[_index].DOScale(Vector3.one , time)); // 중앙의 노드 애니메이션 들어갈 부분
            if (0 <= _index + 1 && _index + 1 < node.Length) animSequence.
                    Join(node[_index + 1].DOScale(Vector3.one, time)); // 오른쪽의 노드 애니메이션 들어갈 부분
            animSequence.OnComplete(() => uiTargetedIndex = _index);
        }
    }

    
    public void Focus(int _index, Vector2 Originps) //여러 개의 캐릭터 창 중 중심 점 잡기
    {
        /*for (int i = 0; i < node.Length; ++i)
        {
            Debug.Log(node[i].anchoredPosition);
        }
        */

        if (0 <= _index && _index < node.Length)
        {
            
            rectTransform.anchoredPosition = (new Vector2 (Originps.x - node[_index].anchoredPosition.x, Originps.y));
            
            for (int i = 0; i < node.Length; ++i)
            {
                if (_index != i)
                    node[i].gameObject.SetActive(false);
            }

        }
    }

    

    public void OnClick_SelectBack() // 캐릭터 창을 닫을 때 나머지 애들 잠금
    {
        
        for (int i = 0 ;i < node.Length;++i)
        {
            if (uiTargetedIndex != i)
                node[i].gameObject.SetActive(false);
        }     
                
    }

    public void OnClick_Select() //캐릭터 창을 열 때 나머지 애들 열기
    {
        for (int i = 0; i < node.Length; ++i)
        {
            if (uiTargetedIndex != i)
                node[i].gameObject.SetActive(true);
        }        
    }

    public void OnClick_Left() //좌 화살표 클릭시 이동 애니메이션 
    {
        
        if (!animSequence.IsActive() && !anim.IsActive())
        {
            BuildAnimation(uiTargetedIndex - 1);
            
            Button btn1 = Stn.GetComponent<Button>();
            Button btn2 = Selectbtn.GetComponent<Button>();

            btn1.enabled = false;
            btn2.enabled = false;
            Invoke("OnInvoke", time + 0.2f);
            
        }
    }

    
    public void OnClick_Right() //우 화살표 클릭시 이동 애니메이션
    {
        
        if (!animSequence.IsActive() && !anim.IsActive())
        {
            BuildAnimation(uiTargetedIndex + 1);
            
            Button btn1 = Stn.GetComponent<Button>();
            Button btn2 = Selectbtn.GetComponent<Button>();

            btn1.enabled = false;
            btn2.enabled = false;
            Invoke("OnInvoke", time + 0.2f);
            
        }
    }

    void OnInvoke() // 캐릭터 창 좌우 이동시 start 버튼과 selectback 버튼 잠금 -> 누를 수 있는 모든 버튼 잠그기
    {
        /*Button btn1 = Stn.GetComponent<Button>();
        btn1.enabled = true;

        Button btn2 = Selectbtn.GetComponent<Button>();
        btn2.enabled = true;
        */
        ButtonLocker();
        GameManager.Instance.NowSelectedPlayerNum = uiTargetedIndex;
    }
    
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡui_char

    public RectTransform rect; 
    public Vector2 OriginPos;
    //public Image image;
    // public Tween tween;

    Sequence anim;

    void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        saveData = save.Load();
    }
    
    
    
    private void Start() // 중심 캐릭터창의 좌표 넣기
    {
        //image.DOFade(0f, 0f);
        OriginPos = rect.anchoredPosition;
        Debug.Log(OriginPos);
        Focus(uiTargetedIndex, OriginPos);
        GunLocker();
    }




    public void OnClick() // 캐릭터 틀을 화면 중심으로 이동하는 버튼
    {
        Button btn1 = Stn.GetComponent<Button>();
        btn1.enabled = false;

        Invoke("OnInvoke", time + 0.2f);
        anim = DOTween.Sequence();

        anim.
            Append(rect.DOScale(2f, 1));//scale을 2로

              

        for(int i = 0; i < node.Length; ++i )
        {
            if (uiTargetedIndex == i)
            {
                anim.
                    Join(rect.DOAnchorPos(new Vector2(- node[i].anchoredPosition.x * 2, 0), 1, false)); // 기존 위치 x2로 이동

            }
        }                       
        //tween = image.DOFade(1f, 1f);
    }



    public void OffClick() //되돌리기
    {
        

        anim = DOTween.Sequence();

        anim.
            Append(rect.DOScale(1f, 1));
        //1배 1초

        for (int i = 0; i < node.Length; ++i)
        {
            if (uiTargetedIndex == i)
            {
                anim.
                    Join(rect.DOAnchorPos(new Vector2(OriginPos.x - node[uiTargetedIndex].anchoredPosition.x, OriginPos.y), 1, false)); // 기존 위치로 이동


            }
        }





       
        //tween = image.DOFade(0f, 1f);

    }
}


// 캐릭터 창이 커지는 거는 애니메이션에 포함되어있지 않음

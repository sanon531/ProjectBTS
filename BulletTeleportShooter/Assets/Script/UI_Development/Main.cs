using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Tools;



public class Main : MonoBehaviour
{
    public Image image;
    public RectTransform rectTransform;
    public GameObject MainMenu;
    public Image[] node;
    public GameObject[] index;
    public RectTransform rects;
    public Ease current_Ease = Ease.InQuad;
    

        

    public void Press()
    {
        
        image.DOFade(0f, 1f);
        Button btn = GetComponent<Button>();
        btn.enabled = false;
        rectTransform.DOAnchorPosY(200f, 1f).SetEase(current_Ease);


        MainMenuSet(); 
    }

    void MainMenuSet()
    {
        node[0].DOFade(1f, 1f).SetDelay(1.5f);
        node[1].DOFade(1f, 1f).SetDelay(1.5f);
        node[2].DOFade(1f, 1f).SetDelay(1.5f);
        
               
        rects.DOAnchorPosY(-280f,1f).SetDelay(1.5f);

        index[1].SetActive(true);


        Invoke("OnInvoke", 2.5f);

    }
   
    void OnInvoke()
    {
                
        Button btn0 = index[0].GetComponent<Button>();
        index[1].GetComponent<MMTouchButton>().enabled = true;
        Button btn2 = index[2].GetComponent<Button>();
        

        btn0.enabled = true;
        
        btn2.enabled = true;
        
    }




}

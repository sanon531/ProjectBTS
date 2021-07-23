using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    private IEnumerator waitThenCallback(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public enum BtnType
    {
        None,
        disableTarget,
        enableTarget,
        pauseGame,
        unPauseGame,
        showMenu,
        hideMenu,
        Start
    }
    public BtnType function;

    public int arg_int = 0;

    // Dotween 적용
    public RectTransform arg_RectTransform;
    public Ease current_Ease = Ease.InBounce;
    public Vector3 ShowPos, HidePos;


    public void Start()
    {
        if(GetComponent<Button>() != null)
            GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        switch (function)
        {
            case BtnType.disableTarget:
                arg_RectTransform.gameObject.SetActive(false);
                break;
            case BtnType.enableTarget:
                arg_RectTransform.gameObject.SetActive(true);
                break;
            case BtnType.showMenu:
                arg_RectTransform.DOAnchorPos(ShowPos, 0.5f).SetEase(current_Ease);
                break;
            case BtnType.hideMenu:
                arg_RectTransform.DOAnchorPos(HidePos, 0.5f).SetEase(current_Ease);
                break;
            case BtnType.pauseGame:
                break;
            case BtnType.unPauseGame:
                Time.timeScale = 1f;
                break;
            case BtnType.None:
                break;
            case BtnType.Start:
                SceneManager.LoadScene("UIScene");
                break;

        }
    }



}

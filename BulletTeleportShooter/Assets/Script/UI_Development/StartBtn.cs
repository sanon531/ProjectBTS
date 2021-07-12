using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        SceneManager.LoadScene("UIScene");
        
        Debug.Log("다음화면");
    }
}
//4개의 맵 구분만 1,2,3,4 uitextpannel
//1번 맵을 1
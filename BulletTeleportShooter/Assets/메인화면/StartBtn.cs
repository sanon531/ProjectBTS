using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        SceneManager.LoadScene("Character");
        
        Debug.Log("다음화면");
    }
}

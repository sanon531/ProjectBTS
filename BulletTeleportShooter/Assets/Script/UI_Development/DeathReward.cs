using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DeathReward : MonoBehaviour
{
    public GameObject score;
    public GameObject time;
    private Text isTime;
    private Text isScore;
    public string mapName;
    public SaveData save;
    public GameObject mapHighIcon;
    public GameObject TimeHighIcon;


    void OnEnable()
    {
        mapName = SceneManager.GetActiveScene().name;
        StartCoroutine(LateShow());
    }
    IEnumerator LateShow()
    {

        yield return new WaitForEndOfFrame();

        isScore = score.GetComponent<Text>();
        float a = float.Parse(isScore.text);

        isTime = time.GetComponent<Text>();
        float b = float.Parse(isTime.text);

        Debug.Log(SaveAndLoad.instance.saveData.mapHigh[mapName] + "+" + a + "+" + b);


        if (a > SaveAndLoad.instance.saveData.mapHigh[mapName].x)
        {
            mapHighIcon.SetActive(true);
        }
        else
        {
            mapHighIcon.SetActive(false);
        }

        if (b > SaveAndLoad.instance.saveData.mapHigh[mapName].y)
        {
            TimeHighIcon.SetActive(true);
        }
        else
        {
            TimeHighIcon.SetActive(false);
        }






        SaveAndLoad.instance.HighScore(mapName, a, b);
        Debug.Log(a);
        Debug.Log(b);


    }

}

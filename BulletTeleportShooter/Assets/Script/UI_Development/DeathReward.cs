using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DeathReward : MonoBehaviour
{
    public GameObject score;
    public GameObject time;
    private Text isTime;
    private Text isScore;
    public string mapName;
    
    

    void OnEnable()
    {
        isScore = score.GetComponent<Text>();
        float a = float.Parse(isScore.text);

        isTime = time.GetComponent<Text>();
        float b = float.Parse(isTime.text);

        SaveAndLoad.instance.HighScore(mapName, a, b);
        Debug.Log(a);
        Debug.Log(b);
    }


}

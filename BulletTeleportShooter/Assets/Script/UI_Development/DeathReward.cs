using System.Collections;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DeathReward : MonoBehaviour
{
    [SerializeField]
    private GameObject score;
    [SerializeField]
    private GameObject time;
    private Text isTime;
    private Text isScore;
    [SerializeField]
    private string mapName;
    [SerializeField]
    private GameObject mapHighIcon;
    [SerializeField]
    private GameObject TimeHighIcon;
    [SerializeField]
    private GameObject AdButtonObj;
    [SerializeField]
    MMTouchButton AdButton;

    public bool CanSeeAD = true;

    private void Awake()
    {
        CanSeeAD = true;
        Debug.Log("Death Awaken");
    }

    void OnEnable()
    {
        mapName = SceneManager.GetActiveScene().name;
        // 부활은 1회만 가능하도록.
        if (CanSeeAD)
        {
            Color tempt = AdButtonObj.GetComponent<Image>().color;
            tempt = new Color(tempt.r, tempt.g, tempt.b, 1f);
            AdButtonObj.GetComponent<Image>().color = tempt;
        }
        else
        {
            Color tempt = AdButtonObj.GetComponent<Image>().color;
            tempt = new Color(tempt.r, tempt.g, tempt.b, 0.4f);
            AdButtonObj.GetComponent<Image>().color = tempt;
            AdButton.enabled = false;
        }
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
        
    }


    public void DisableAdButton()
    {
        Debug.Log("Disabled");
        CanSeeAD = false;
    }

}

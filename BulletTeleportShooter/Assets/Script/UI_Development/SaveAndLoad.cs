﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


[System.Serializable]

public class SaveData
{
    [SerializeField]
    public bool tutorialOn = true;

    [SerializeField]
    public int lastPlayedMaps = 0;

    [SerializeField]
    public int lastUsedGuns  = 0;

    [SerializeField]
    public MapLock mapLock;

    [SerializeField]
    public GunLock gunLock;

    [SerializeField]
    public MapHigh mapHigh;

    [SerializeField]
    public GunHigh gunHigh;

    public int gold;

    
}


public class SaveAndLoad : MonoBehaviour
{

    [SerializeField]
    public SaveData saveData = new SaveData();

    public static SaveAndLoad instance;

    public bool isStart = false;

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";
    
    

    void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
        if (instance==null)
            instance = this;

    }
    void Start()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME) == true)
        {
            Load();
        }
    }

    public void InitialSave()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME) == false)
        {
            string jsonData = JsonUtility.ToJson(saveData);

            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, jsonData);

            Debug.Log(jsonData);

        }

    }

    public void HighScore(string _name ,float _score,float _time)
    {
        if(_score > saveData.mapHigh[_name].x)
        {
            saveData.mapHigh[_name] = new Vector2(_score, saveData.mapHigh[_name].y);
        }
        if(_time > saveData.mapHigh[_name].y)
        {
            saveData.mapHigh[_name] = new Vector2(saveData.mapHigh[_name].x, _time);
        }
        
        Save();
    }

    public bool UnLockByName(bool isgun,string _name)
    {
       
        if (isgun)
        {
            if (saveData.gunLock[_name])
            {
                return false;
            }
            else
            {
                saveData.gunLock[_name] = true;
                Save();

                return true;
            }
        }
        else
        {
            if (saveData.mapLock[_name])
            {
                return false;
            }
            else
            {
                saveData.mapLock[_name] = true;
                Save();
                return true;
            }
        }
    }
    
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveData);
        
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, jsonData);

        Debug.Log("저장 완료");
        Debug.Log(jsonData);
    }

    public SaveData Load()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);
            Debug.Log(saveData);
            
        }
        return saveData;
    }

    public void SetLastMap(int mapNum)
    {
        saveData.lastPlayedMaps = mapNum;
        Save();
    }
    public void SetLastGun(int gunNum)
    {
        saveData.lastUsedGuns = gunNum;
        Save();

    }

}



//맵에서 보상 웨이브를 클리어 하면 맵이 해금되고 여기 세이브앤 로드 파일에 불리언의 false를 
//true로 바꾸고 uiscene에서 눌렀을때 열리도록 만드는 함수를 추가해보자!
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


[System.Serializable]

public class SaveData
{
   
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
    

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";
    
    

    void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        if (instance==null)
            instance = this;

    }
    void Start()
    {
        Load();

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
    

}



//맵에서 보상 웨이브를 클리어 하면 맵이 해금되고 여기 세이브앤 로드 파일에 불리언의 false를 
//true로 바꾸고 uiscene에서 눌렀을때 열리도록 만드는 함수를 추가해보자!
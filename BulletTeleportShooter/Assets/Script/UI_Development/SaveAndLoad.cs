using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;




public class SaveAndLoad : MonoBehaviour
{

    
    

    [SerializeField]
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveData);
        //string path = Path.Combine(SAVE_DATA_DIRECTORY, SAVE_FILENAME);
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, jsonData);



        Debug.Log("저장 완료");
        Debug.Log(jsonData);
    }

    public void Load()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);


            Debug.Log(saveData);
        }
    }


}

[System.Serializable]

public class SaveData
{
    [SerializeField]
    MapLock mapLock;

    [SerializeField]
    GunLock gunLock;

    [SerializeField]
    MapHigh mapHigh;

    [SerializeField]
    GunHigh gunHigh;

    public int gold;
}

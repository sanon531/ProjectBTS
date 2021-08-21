using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


[System.Serializable]

public class SaveData
{
    /*[SerializeField]
    public ObjectColorDictionary m_objectColorDictionary;

    [SerializeField]
    StringStringDictionary m_stringStringDictionary;
    public IDictionary<string,string> StringStringDictionary
    {
        get { return m_stringStringDictionary; }
        set { m_stringStringDictionary.CopyFrom(value); }
    }*/


    [SerializeField]
    public MapLock mapLock;

    [SerializeField]
    public GunLock gunLock;

    [SerializeField]
    MapHigh mapHigh;

    [SerializeField]
    GunHigh gunHigh;

    public int gold;
}


public class SaveAndLoad : MonoBehaviour
{



    /*[SerializeField]
    StringStringDictionary m_stringStringDictionary;
    public IDictionary<string, string> StringStringDictionary
    {
        get { return m_stringStringDictionary; }
        set { m_stringStringDictionary.CopyFrom(value); }
    }
    */

    
    public string[] mapName;
    public GameObject[] images;
    
    public GameObject selectButton;

    [SerializeField]
    private SaveData saveData = new SaveData();

    

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    

    void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        Load();
    }
    void Start()
    {
        
        MapLocker();
        
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

  
    public void MapLocker()
    {
              
        MapLock mapLock = saveData.mapLock;
        foreach(var keyValuePair in mapLock)
        {
            
            for (int i=0;i < mapName.Length; i++)
            {
                                
                if (keyValuePair.Key == mapName[i])
                {
                    if (keyValuePair.Value)
                    {
                        //selectButton.GetComponent<Button>().enabled = true;
                        images[i].SetActive(false);
                        
                    }
                    else
                    {
                        //selectButton.GetComponent<Button>().enabled = false;
                        images[i].SetActive(true);
                        
                    }
                }

            }

        }     


    }
    /*public void Prac()
    {
       for(int i =0; i< images.Length; i++)
        {
            if (images[i] == true)
            {
                Debug.Log(i);
                Debug.Log("액티브함");
            }

            else
            {
                Debug.Log(i);
                Debug.Log("언액티브함");
            }
                
        }
    }*/

}



//맵에서 보상 웨이브를 클리어 하면 맵이 해금되고 여기 세이브앤 로드 파일에 불리언의 false를 
//true로 바꾸고 uiscene에서 눌렀을때 열리도록 만드는 함수를 추가해보자!
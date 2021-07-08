using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectManager : MonoBehaviour
{

    [SerializeField]
    List <BTS_Object_Data> OnOffBlockList = new List <BTS_Object_Data> ();

    public static MapObjectManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void AddOnOff(BTS_Object_Data data)
    {
        OnOffBlockList.Add(data);
    }

    public float timer = 0f;
    //float timer = 0f;
    // Update is called once per frame
    void Update()
    {

        //Timer Timer_GameTime = GameObject.Find("GameManager").GetComponent<Timer>();
        //timer = Timer_GameTime.GameTime;
        
        timer += Time.deltaTime;

        if (timer > 5f)
        {
            for (int i = 0; i<OnOffBlockList.Count; i++)
            {
                OnOffBlockList[i].OnOff();
            }
            timer = 0f;
        }
        
    }
}

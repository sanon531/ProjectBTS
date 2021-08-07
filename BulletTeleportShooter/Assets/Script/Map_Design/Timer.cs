using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour
{
    // Start is called before the first frame update

    public float GameTime = 0;
    public Text GameTimeText;

    public void BTS_OnClickBotton()
    {
        GameTime = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        GameTime += Time.deltaTime;
        
        GameTimeText.text = "Time : " + (int)GameTime;

        
        if (SceneManager.GetActiveScene().name != "RealFirst")
        {
            GameTime = 0;
            GameObject.Find("SpikeObject").GetComponent<SpikeObjectManager>().spike_speed = 1f;
        }
        
    }


}

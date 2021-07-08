using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update

    public float GameTime = 0;
    public Text GameTimeText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += Time.deltaTime;
        Debug.Log((int)GameTime);
        GameTimeText.text = "Time : " + (int)GameTime;
    }
}

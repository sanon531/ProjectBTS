using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class TimerCount : MonoBehaviour
    {
        private float timeCount = 0;
        private Text _text; 

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void Update()
        {
            timeCount += Time.deltaTime;
            _text.text = ((int)timeCount).ToString();
            
        }
    }
}
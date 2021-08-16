using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class TimeCount : MonoBehaviour
    {
        private int timeCount = 0;
        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
            StartCoroutine("TimeCoroutine");
        }

        IEnumerator TimeCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            timeCount++;
            GameManager.Instance.TimeCount = timeCount;
            _text.text = timeCount.ToString();
            StartCoroutine("TimeCoroutine");
        }
    }
}
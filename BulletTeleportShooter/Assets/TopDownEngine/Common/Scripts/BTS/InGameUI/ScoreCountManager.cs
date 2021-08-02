using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class ScoreCountManager : MMSingleton<ScoreCountManager>
    {
        [Tooltip("시간 증가에 따른 점수 상승값")]
        public int OneTimeScoreUpAmount = 10;

        private int timeCount = 0;

        private void Start()
        {
            StartCoroutine("OneSecondCoroutine");
        }

        IEnumerator OneSecondCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            GameManager.Instance.AddPoints(OneTimeScoreUpAmount);
            if (++timeCount % 10 == 0) OneTimeScoreUpAmount += 10;
            StartCoroutine("OneSecondCoroutine");
        }     
    }
}
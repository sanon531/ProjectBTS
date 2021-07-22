using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class ScoreUpItem : MonoBehaviour
    {
        [Tooltip("아이템 획득 시 점수 증가량")]
        public int ScoreUpAmount;

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                ScoreUpAmount = ScoreCountManager.Instance.OneTimeScoreUpAmount * 10;
                GameManager.Instance.AddPoints(ScoreUpAmount);
            }
        }
    }
}
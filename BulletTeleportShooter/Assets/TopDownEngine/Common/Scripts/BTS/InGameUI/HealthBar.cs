using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class HealthBar : MonoBehaviour
    {
        private GameObject Player;
        private BTS_CharacterManager _characterManager;
        private int nowHealth;
        private int maxHealth;
        private GameObject[] HealthBars;

        public GameObject HealthWarning;
        public MMFeedbacks HealthWarningFeedback;


        private void Awake()
        {
            Player = LevelManager.Instance.PlayerPrefabs[0].gameObject;
            _characterManager = Player.GetComponent<BTS_CharacterManager>();
            Initialization();
        }

        private void Update()
        {
            if (!HealthWarningFeedback.IsPlaying)
            {
                HealthWarning.SetActive(false);
            }
        }

        public void Initialization()
        {
            nowHealth = _characterManager.InitialHealth;
            maxHealth = _characterManager.MaximumHealth;
            HealthBars = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                HealthBars[i] = transform.GetChild(i).gameObject;
                HealthBars[i].SetActive(false);
            }
            
            for (int i = 0; i < nowHealth; i++)
            {
                HealthBars[i].SetActive(true);
            }

            StartCoroutine(SetWarning(true));
        }

        public void RemoveHealthBar(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (!isHealthZero())
                {
                    HealthBars[nowHealth - 1].SetActive(false);
                    nowHealth--;
                }
            }

            if (!isHealthZero())
            {
                StartCoroutine(SetWarning(false));
            }
        }

        public void MakeHealthBar(int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (!isHealthMax())
                {
                    HealthBars[nowHealth].SetActive(true);
                    nowHealth++;
                }
            }

            StartCoroutine(SetWarning(false));
        }

        private bool isHealthMax()
        {
            return nowHealth >= maxHealth;
        }

        private bool isHealthZero()
        {
            return nowHealth <= 0;
        }


        private IEnumerator SetWarning(bool Initial)
        {
            yield return new WaitForEndOfFrame();

            RectTransform r = HealthWarning.GetComponent<RectTransform>();
            RectTransform t = HealthBars[nowHealth - 1].GetComponent<RectTransform>();

            r.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y);
            
            /*if (SpawnManager.Instance.currentPowerUpATK + 1 >= nowHealth)
            {
                GUIManager.Instance.btsHealthBar.GetComponent<HealthBar>().HealthWarning.SetActive(true);
                HealthWarningFeedback.PlayFeedbacks();
            }*/

            if (!Initial)
            {
                HealthWarning.SetActive(true);
                HealthWarningFeedback?.PlayFeedbacks();
            }
        }
    }
}
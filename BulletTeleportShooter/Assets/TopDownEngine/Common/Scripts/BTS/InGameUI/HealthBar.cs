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

        private void Start()
        {
            Player = LevelManager.Instance.PlayerPrefabs[0].gameObject;
            _characterManager = Player.GetComponent<BTS_CharacterManager>();
            Initialization();
        }
        private void Initialization()
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
        }

        private bool isHealthMax()
        {
            return nowHealth >= maxHealth;
        }

        private bool isHealthZero()
        {
            return nowHealth <= 0;
        }
    }
}
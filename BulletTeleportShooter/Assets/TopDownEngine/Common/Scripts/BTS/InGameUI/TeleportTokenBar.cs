using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class TeleportTokenBar : MonoBehaviour
    {
        private GameObject Player;
        private BTS_CharacterManager _characterManager;
        private int nowTokenBar;
        private GameObject[] TokenBars;

        public Sprite FilledBarSprite;
        public Sprite BlankBarSprite;

        private void Awake()
        {
            Player = LevelManager.Instance.PlayerPrefabs[0].gameObject;
            _characterManager = Player.GetComponent<BTS_CharacterManager>();
        }

        private void Start()
        {
            Initialization();
        }
        private void Initialization()
        {
            nowTokenBar = _characterManager.MaxTeleportToken;
            TokenBars = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                TokenBars[i] = transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < nowTokenBar; i++)
            {
                TokenBars[i].SetActive(true);
            }
        }

        public void RemoveTokenBar(int tokenAmount)
        {
            for (int i = 0; i < tokenAmount; i++)
            {
                if (!isTokenZero())
                {
                    TokenBars[nowTokenBar - 1].GetComponent<Image>().sprite = BlankBarSprite;
                    nowTokenBar--;
                }
            }
        }

        public void MakeTokenBar(int tokenAmount)
        {
            for (int i = 0; i < tokenAmount; i++)
            {
                if (!isTokenMax())
                {
                    TokenBars[nowTokenBar].GetComponent<Image>().sprite = FilledBarSprite;
                    nowTokenBar++;
                }
            }        
        }

        private bool isTokenMax()
        {
            return nowTokenBar >= _characterManager.MaxTeleportToken;
        }

        private bool isTokenZero()
        {
            return nowTokenBar <= 0;
        }
    }
}
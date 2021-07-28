using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class TeleportTokenBar : MonoBehaviour
    {
        private GameObject Player;
        private BTS_CharacterManager _characterManager;
        private int nowTokenNum;
        private GameObject[] Tokens;

        [Header("Teleport Token UI Setting")]
        public GameObject TeleportTokensUI;
        public Sprite FilledTokenSprite;
        public Sprite BlankTokenSprite;

        [Header("Teleport Token Bar UI Setting")]
        public GameObject TeleportTokenBarUI;
        public GameObject FilledBarUI;
        public float BarBaseWidth = 1.0f;
        public float BarBaseHeight = 1.0f;
       

        private void Start()
        {
            Player = LevelManager.Instance.PlayerPrefabs[0].gameObject;
            _characterManager = Player.GetComponent<BTS_CharacterManager>();
            Initialization();
        }
        private void Initialization()
        {
            nowTokenNum = _characterManager.MaxTeleportToken;
            Tokens = new GameObject[TeleportTokensUI.transform.childCount];

            for (int i = 0; i < TeleportTokensUI.transform.childCount; i++)
            {
                Tokens[i] = TeleportTokensUI.transform.GetChild(i).gameObject;
                Tokens[i].SetActive(false);
            }

            for (int i = 0; i < nowTokenNum; i++)
            {
                Tokens[i].SetActive(true);
            }

            SetTokenBar();
        }

        public void RemoveToken(int tokenAmount)
        {
            for (int i = 0; i < tokenAmount; i++)
            {
                if (!isTokenZero())
                {
                    Tokens[nowTokenNum - 1].GetComponent<Image>().sprite = BlankTokenSprite;
                    nowTokenNum--;
                }
            }
        }

        public void MakeToken(int tokenAmount)
        {
            for (int i = 0; i < tokenAmount; i++)
            {
                if (!isTokenMax())
                {
                    Tokens[nowTokenNum].GetComponent<Image>().sprite = FilledTokenSprite;
                    nowTokenNum++;
                }
            }        
        }

        private void SetTokenBar()
        {
            TeleportTokenBarUI.GetComponent<RectTransform>().localScale = new Vector3(_characterManager.MaxTeleportToken * BarBaseWidth, BarBaseHeight, 1);
            TeleportTokenBarUI.SetActive(true);
            
            
        } 

        private bool isTokenMax()
        {
            return nowTokenNum >= _characterManager.MaxTeleportToken;
        }

        private bool isTokenZero()
        {
            return nowTokenNum <= 0;
        }
    }
}
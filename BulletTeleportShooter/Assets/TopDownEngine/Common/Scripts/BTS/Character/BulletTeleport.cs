using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class BulletTeleport : CharacterAbility
    {
        private BulletTeleportManager _bulletTeleportManager;
        private LinkedList<GameObject> bulletStack;
        private GameObject TargetBullet;
        private BTS_CharacterManager _characterManager;

        [Header("Invulner Time")]
        [Tooltip("텔레포트 후 무적시간")]
        public float InvulnerTime;

        [Header("Teleport Token")]
        [Tooltip("텔레포트 1회당 토큰 사용량")]
        public int UseTokenAmount = 1;
        [Tooltip("토큰 충전 시간 간격")]
        public float TokenRechargeDelay = 2.0f;
        [Tooltip("1회 토큰 충전량")]
        public int TokenRechargeAmount = 1;

        private int nowToken;
        private int MaxTeleportToken;
        private float nowDelay;

        private TeleportTokenBar _teleportTokenBar;
        private Image TeleportTokenBarImage;

        [Header("Teleport Effect")]
        [Tooltip("the feedback to play when Teleport")]
        public MMFeedbacks TeleportFeedback;
        public MMObjectPooler ObjectPooler;

        [Tooltip("the feedback to play when Teleport Cannot Work")]
        public MMFeedbacks CannotTeleportFeedback;


        protected override void Initialization()
        {
            base.Initialization();
            _bulletTeleportManager = BulletTeleportManager.Instance;
            bulletStack = _bulletTeleportManager.BulletStack;
            _characterManager = GetComponent<BTS_CharacterManager>();
            _health = GetComponent<Health>();
            MaxTeleportToken = _characterManager.MaxTeleportToken;
            nowToken = MaxTeleportToken;
            _teleportTokenBar = GUIManager.Instance.TeleportTokenBar;
            TeleportTokenBarImage = _teleportTokenBar.FilledBarUI.GetComponent<Image>();
            CannotTeleportFeedback.GetComponent<MMFeedbackPosition>().AnimatePositionTarget = GUIManager.Instance.TeleportTokenBar.gameObject;
            CannotTeleportFeedback.GetComponent<MMFeedbackCanvasGroup>().TargetCanvasGroup = GUIManager.Instance.TeleportTokenBar.GetComponent<CanvasGroup>();
        }

        protected override void HandleInput()     //점멸 조작키 설정
        {
            base.HandleInput();
            if (!AbilityAuthorized
                || (_condition.CurrentState != CharacterStates.CharacterConditions.Normal))
            {
                return;
            }
            if (_inputManager.SecondaryShootButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                TeleportTry();
            }
        }

        public void TeleportTry()
        {
            if (isTeleportEnable())
            {
                TeleportStart();
            }
            else
            {
                CannotTeleportFeedback?.PlayFeedbacks();
            }
        }

        private void TeleportStart()
        {
            if (bulletStack == null || bulletStack.Count == 0)
            {
                TargetBullet = null;
            }
            else
            {
                UseTeleportToken(UseTokenAmount);

                StopCoroutine("TokenRechargeCoroutine");
                StartCoroutine("TokenRechargeCoroutine");

                RemoveBullet();
                transform.position = TargetBullet.transform.position;

                _health.DamageDisabled();                //점멸 후 플레이어 잠시 무적
                TeleportFeedback?.PlayFeedbacks(this.transform.position);

                GetComponent<Explosion>().explode();

                if (ObjectPooler != null)
                {
                    SpawnCrack();
                }

                Invoke("InvulnerDelay", InvulnerTime);      //데미지 입힌 후 1.5초 뒤 무적 해제
            }
        }

        private void InvulnerDelay()
        {
            _health.DamageEnabled();
        }

        private void RemoveBullet()
        {
            TargetBullet = bulletStack.Last.Value;
            bulletStack.RemoveLast();
            TargetBullet.SetActive(false);
        }

        public void UseTeleportToken(int useAmount)
        {
            nowToken -= useAmount;

            _teleportTokenBar.RemoveToken(useAmount); 
        }

        public void TeleportTokenCharge(int chargeAmount)
        {
            nowToken += chargeAmount;

            if (nowToken > MaxTeleportToken)
            {
                nowToken = MaxTeleportToken;
            }


            _teleportTokenBar.MakeToken(chargeAmount);
        }

        IEnumerator TokenRechargeCoroutine()
        {
            nowDelay = TokenRechargeDelay;

            while (nowDelay > 0)
            {
                nowDelay -= Time.deltaTime;
                TeleportTokenBarImage.fillAmount = (1 - nowDelay / TokenRechargeDelay);

                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForSeconds(TokenRechargeDelay);

            nowToken += TokenRechargeAmount;

            if (nowToken > MaxTeleportToken)
            {
                nowToken = MaxTeleportToken;
            }

            _teleportTokenBar.MakeToken(TokenRechargeAmount);
            TeleportTokenBarImage.fillAmount = 0;

            if (!isTokenMax())
            {
                StartCoroutine("TokenRechargeCoroutine");
            }
        }

        private bool isTeleportEnable()
        {
            return nowToken >= UseTokenAmount;
        }

        private bool isTokenMax()
        {
            return nowToken >= MaxTeleportToken;
        }

        private void SpawnCrack()
        {
            GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

            // mandatory checks
            if (nextGameObject == null) { return; }

            nextGameObject.GetComponent<ParticleSystem>().Play();

            // we position the object
            nextGameObject.transform.position = transform.position + new Vector3(0, 0, -3f); 

            // we activate the object
            nextGameObject.gameObject.SetActive(true);

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }
    }
}
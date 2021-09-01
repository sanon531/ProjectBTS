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
        private Vector2 TargetPos;

        private TeleportTokenBar _teleportTokenBar;
        private Image TeleportTokenBarImage;

        [Header("Teleport Effect")]
        [Tooltip("the feedback to play when Teleport")]
        public MMFeedbacks TeleportFeedback;
        public MMObjectPooler ObjectPooler;

        [Tooltip("the feedback to play when Teleport Cannot Work")]
        public MMFeedbacks CannotTeleportFeedback;
        [Tooltip("the radius to our Teleport Target ")]
        public float Radius = 3f;
        protected bool _init = false;


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
            CannotTeleportFeedback.GetComponent<MMFeedbackImage>().BoundImage = GUIManager.Instance.TeleportTokenBar.GetComponent<TeleportTokenBar>().TokenWarning.GetComponent<Image>();
            Radius = _characterManager.FlashRange;
            _init = true;

        }

        private void Update()
        {
            if (!CannotTeleportFeedback.IsPlaying)
            {
                GUIManager.Instance.TeleportTokenBar.GetComponent<TeleportTokenBar>().TokenWarning.SetActive(false);
            }
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
                GUIManager.Instance.TeleportTokenBar.GetComponent<TeleportTokenBar>().TokenWarning.SetActive(true);
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
                SetTargetPos();
                transform.position = new Vector2(TargetPos.x, TargetPos.y);

                _health.DamageDisabled();                //점멸 후 플레이어 잠시 무적
                TeleportFeedback?.PlayFeedbacks(this.transform.position);

                GetComponent<Explosion>().explode();

                if (ObjectPooler != null)
                {
                    SpawnCrack();
                    if (_movement.CurrentState != CharacterStates.MovementStates.Teleporting)
                    {
                        _movement.ChangeState(CharacterStates.MovementStates.Teleporting);
                        //Debug.Log("tt");
                    }
                }
                Invoke("InvulnerDelay", InvulnerTime);      //데미지 입힌 후 1.5초 뒤 무적 해제
                Invoke("AnimationDelay", 0.1f);
            }
        }

        private void InvulnerDelay()
        {
            _health.DamageEnabled();
        }
        private void AnimationDelay()
        {
            _movement.ChangeState(CharacterStates.MovementStates.Idle);
        }

        private void RemoveBullet()
        {
            TargetBullet = bulletStack.Last.Value;
            BulletTeleportManager.Instance.DeleteBullet(TargetBullet);
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

        private void SetTargetPos()
        {
            TargetPos = new Vector2(TargetBullet.transform.position.x, TargetBullet.transform.position.y);
            TargetPos.x = Mathf.Clamp(TargetPos.x, _bulletTeleportManager.leftDown.position.x, _bulletTeleportManager.rightUp.position.x);
            TargetPos.y = Mathf.Clamp(TargetPos.y, _bulletTeleportManager.leftDown.position.y, _bulletTeleportManager.rightUp.position.y);
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

        public void ResetTokenToMaxToken()
        {
            nowToken = MaxTeleportToken;
            _teleportTokenBar.Initialization();
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

            nextGameObject.transform.localScale = new Vector3(Radius, Radius, Radius);

            ParticleSystem temptParticle = nextGameObject.GetComponent<ParticleSystem>();
            temptParticle.Play();

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
        protected virtual void OnDrawGizmosSelected()
        {

            Gizmos.color = Color.blue;

            Color _gizmoColor = Gizmos.color;

            Gizmos.DrawWireSphere(transform.position, Radius);

            if (_init)
            {
                _gizmoColor.a = 0.25f;
                Gizmos.color = _gizmoColor;
                Gizmos.DrawSphere(transform.position, Radius);
            }
        }
    }
}
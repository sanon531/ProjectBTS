using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Collections;

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

        [Header("Overload")]
        [Tooltip("텔레포트 1회당 과부하 게이지 증가량")]
        public int OneTimeOverloadUP = 20;
        [Tooltip("과부하 감소 시간 간격")]
        public float OverloadDownDelay = 0.1f;
        [Tooltip("1회 과부하 게이지 감소량")]
        public int OneTimeOverloadDown = 1;

        private int nowOverload = 0;
        private bool TeleportEnable = true;
        private int MaxOverload;
        private bool coroutineIsRunning = false;
        
        [Header("Teleport Effect")]
        [Tooltip("the feedback to play when Teleport")]
        public MMFeedbacks TeleportFeedback;
        public MMObjectPooler ObjectPooler;


        protected override void PreInitialization()
        {
            base.PreInitialization();
            _bulletTeleportManager = BulletTeleportManager.Instance;
            bulletStack = _bulletTeleportManager.BulletStack;
            _characterManager = GetComponent<BTS_CharacterManager>();
            _health = GetComponent<Health>();
            MaxOverload = _characterManager.MaxOverload;
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
                if (TeleportEnable)
                {
                    TeleportStart();
                }
            }
        }

        private void TeleportStart()
        {
            if (bulletStack.Count == 0)
            {
                TargetBullet = null;
            }
            else
            {
                OverloadUP(OneTimeOverloadUP);
                if (!coroutineIsRunning)
                {
                    StartCoroutine("OverloadDownCoroutine", OverloadDownDelay);
                }

                RemoveBullet();
                transform.position = TargetBullet.transform.position;

                _health.Invulnerable = true;                //점멸 후 플레이어 잠시 무적
                TeleportFeedback?.PlayFeedbacks(this.transform.position);

                GetComponent<Explosion>().explode();

                if (ObjectPooler != null)
                {
                    SpawnCrack();
                }

                Invoke("InvulnerDelay", InvulnerTime);       //데미지 입힌 후 1.5초 뒤 무적 해제
            }
        }

        private void InvulnerDelay()
        {
            _health.Invulnerable = false;
        }

        private void RemoveBullet()
        {
            TargetBullet = bulletStack.Last.Value;
            bulletStack.RemoveLast();
            TargetBullet.SetActive(false);
        }

        public void OverloadUP(int upAmount)
        {
            nowOverload += upAmount;
            if (nowOverload > MaxOverload - OneTimeOverloadUP)
            {
                TeleportEnable = false;
            }

            //테스트 용 출력  KoalaUICamera -> OverloadTextManager.cs -> OverloadCounter (Text)
            OverloadTextManager.Instance.SetOverloadText(nowOverload);      
        }

        public void OverloadDown(int downAmount)
        {
            nowOverload -= downAmount;

            if (nowOverload <= MaxOverload - OneTimeOverloadUP)
            {
                TeleportEnable = true;
            }
            else
            {
                TeleportEnable = false;
            }

            OverloadTextManager.Instance.SetOverloadText(nowOverload);      //테스트 용 출력
        }

        IEnumerator OverloadDownCoroutine()
        {
            coroutineIsRunning = true;
            nowOverload -= OneTimeOverloadDown;
            if (nowOverload <= MaxOverload - OneTimeOverloadUP)
            {
                TeleportEnable = true;
            }
            else
            {
                TeleportEnable = false;
            }

            OverloadTextManager.Instance.SetOverloadText(nowOverload);      //테스트 용 출력

            yield return new WaitForSeconds(OverloadDownDelay);

            coroutineIsRunning = false;
            if (nowOverload > 0)
            {
                StartCoroutine("OverloadDownCoroutine");
            }
        }


        private void SpawnCrack()
        {
            GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

            // mandatory checks
            if (nextGameObject == null) { return; }

            nextGameObject.GetComponent<ParticleSystem>().Play();

            // we position the object
            nextGameObject.transform.position = transform.position;

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
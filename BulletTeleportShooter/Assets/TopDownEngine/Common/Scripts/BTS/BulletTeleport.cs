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

        public float InvulnerTime;

        public float OverloadDownDelay = 0.1f;
        public int OneTimeOverloadUP = 20;
        public int OneTimeOverloadDown = 1;

        private int nowOverload = 0;
        private bool TeleportEnable = true;
        private int MaxOverload;
        private bool coroutineIsRunning = false;
        

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
                OverloadUP();
                if (!coroutineIsRunning)
                {
                    StartCoroutine("OverloadDown", OverloadDownDelay);
                }

                RemoveBullet();
                transform.position = TargetBullet.transform.position;

                _health.Invulnerable = true;        //점멸 후 플레이어 잠시 무적
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
            //RigidBody 없애는 방식은 이후에 오류 발생 시킴
        }
        
        private void OverloadUP()
        {
            nowOverload += OneTimeOverloadUP;
            if (nowOverload > MaxOverload - OneTimeOverloadUP)
            {
                TeleportEnable = false;
            }
        }

        IEnumerator OverloadDown()
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

            //Debug.Log(nowOverload);         //테스트 용 출력

            yield return new WaitForSeconds(OverloadDownDelay);

            coroutineIsRunning = false;
            if (nowOverload > 0)
            {
                StartCoroutine("OverloadDown");
            }
        }

        private void SpawnCrack()
        {
            GameObject nextGameObject = ObjectPooler.GetPooledGameObject();

            // mandatory checks
            if (nextGameObject == null) { return; }

            nextGameObject.GetComponent<ParticleSystem>().Play();

            // we position the object
            nextGameObject.transform.position = transform.position+ new Vector3(0,0,-3f);

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
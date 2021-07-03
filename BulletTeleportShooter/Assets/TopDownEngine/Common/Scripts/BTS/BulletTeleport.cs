using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class BulletTeleport : CharacterAbility
    {
        private BulletTeleportManager _bulletTeleportManager;
        private LinkedList<GameObject> bulletStack;
        private GameObject TargetBullet;

        public float InvulnerTime;
      

        protected override void PreInitialization()
        {
            base.PreInitialization();
            _bulletTeleportManager = GameObject.Find("TeleportManager").GetComponent<BulletTeleportManager>();
            bulletStack = _bulletTeleportManager.BulletStack;
            _health = GetComponent<Health>();
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
                TeleportStart();
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
                RemoveBullet();
                transform.position = TargetBullet.transform.position;
                
                _health.Invulnerable = true;        //점멸 후 플레이어 잠시 무적
                
                GetComponent<Explosion>().explode();

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
    }
}
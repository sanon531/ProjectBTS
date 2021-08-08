using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class btsHealth : Health
    {
        private HealthBar _btsHealthBar;

        protected override void Awake()
        {
            base.Initialization();
            _btsHealthBar = GUIManager.Instance.btsHealthBar;
        }

        public override void GetHealth(int health, GameObject instigator)
        {
            base.GetHealth(health, instigator);
            _btsHealthBar.MakeHealthBar(health);
        }

        public override void ResetHealthToMaxHealth()
        {
            base.ResetHealthToMaxHealth();
            _btsHealthBar.Initialization();
        }

        public override void Damage(int damage, GameObject instigator, float flickerDuration, float invincibilityDuration, Vector3 damageDirection)
        {
            if (Invulnerable)
            {
                return;
            }

            // if we're already below zero, we do nothing and exit
            if ((CurrentHealth <= 0) && (InitialHealth != 0))
            {
                return;
            }

            // we decrease the character's health by the damage
            float previousHealth = CurrentHealth;
            CurrentHealth -= damage;

            _btsHealthBar.RemoveHealthBar(damage);      //추가

            LastDamage = damage;
            LastDamageDirection = damageDirection;
            if (OnHit != null)
            {
                OnHit();
            }

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }

            // we prevent the character from colliding with Projectiles, Player and Enemies
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // we trigger a damage taken event
            MMDamageTakenEvent.Trigger(_character, instigator, CurrentHealth, damage, previousHealth);

            if (_animator != null)
            {
                _animator.SetTrigger("Damage");
            }

            DamageMMFeedbacks?.PlayFeedbacks(this.transform.position, damage);

            // we update the health bar
            UpdateHealthBar(true);

            // if health has reached zero
            if (CurrentHealth <= 0)
            {
                // we set its health to zero (useful for the healthbar)
                CurrentHealth = 0;

                Kill();
            }
        }

    }
}
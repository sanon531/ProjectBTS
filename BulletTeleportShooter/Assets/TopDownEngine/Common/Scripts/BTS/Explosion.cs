using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class Explosion : MonoBehaviour
    {
        public int radius = 0;
        public float force;

        public LayerMask LayerToHit;

        private Health _health;
        private BTS_CharacterManager _characterManager;

        void Awake()
        {
            _characterManager = GetComponent<BTS_CharacterManager>();
        }

        public void explode()
        {
            radius = _characterManager.flashRange;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, LayerToHit);

            foreach (Collider2D col in colliders)
            {
                Vector2 direction = col.transform.position - transform.position;
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    _health = col.gameObject.GetComponent<Health>();

                    rb.AddForce(direction * force);                                                             //밀기
                    _health.Damage(_characterManager.flashDMG, this.gameObject, 0.5f, 0.5f, Vector3.zero);      //데미지 입히기
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
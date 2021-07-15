using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class Explosion : MonoBehaviour
    {
        [Tooltip("폭발 범위 (반지름)")]
        public int Radius = 0;

        [Tooltip("폭발 시 주변 오브젝트를 미는 힘")]
        public float Force;

        [Tooltip("폭발 시 밀리는 대상 Layer")]
        public LayerMask LayerToHit;

        private Health _health;
        private BTS_CharacterManager _characterManager;

        void Awake()
        {
            _characterManager = GetComponent<BTS_CharacterManager>();
        }

        public void explode()
        {
            Radius = _characterManager.FlashRange;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius, LayerToHit);

            foreach (Collider2D col in colliders)
            {
                Vector2 direction = col.transform.position - transform.position;
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    _health = col.gameObject.GetComponent<Health>();

                    rb.AddForce(direction * Force);                                                             //밀기
                    _health.Damage(_characterManager.FlashDMG, this.gameObject, 0.5f, 0.5f, Vector3.zero);      //데미지 입히기
                }
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
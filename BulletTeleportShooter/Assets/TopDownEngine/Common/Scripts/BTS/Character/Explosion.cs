using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class Explosion : MonoBehaviour
    {
        [Tooltip("폭발 범위 반지름, 실제 반지름은 캐릭터 설정 값으로 결정되므로 여기 값은 범위 확인용으로만 사용")]
        public int Radius = 0;

        [Tooltip("폭발 시 주변 오브젝트를 미는 힘")]
        public float Force;

        [Tooltip("해당 숫자 만큼 폭발 범위 Y축 위치 수정")]
        public float PosY;

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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y + PosY), Radius, LayerToHit);

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
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y + PosY), Radius);
        }
    }
}
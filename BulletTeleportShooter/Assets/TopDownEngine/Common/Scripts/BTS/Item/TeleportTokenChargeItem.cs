using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class TeleportTokenChargeItem : MonoBehaviour
    {
        [Tooltip("아이템 획득 시 텔레포트 토큰 충전량")]
        public int TokenChargeAmount;

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<BulletTeleport>().TeleportTokenCharge(TokenChargeAmount);
            }
        }
    }
}
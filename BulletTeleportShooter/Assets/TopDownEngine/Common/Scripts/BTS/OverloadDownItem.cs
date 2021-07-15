using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class OverloadDownItem : MonoBehaviour
    {
        [Tooltip("아이템 획득 시 과부하 게이지 감소량")]
        public int OverloadDownAmount;

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<BulletTeleport>().OverloadDown(OverloadDownAmount);
            }
        }
    }
}
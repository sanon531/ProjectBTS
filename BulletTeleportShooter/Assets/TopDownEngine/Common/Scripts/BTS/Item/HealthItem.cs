using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class HealthItem : MonoBehaviour
    {
        [Tooltip("아이템 획득 시 체력 회복량")]
        public int HealAmount;

        public virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<Health>().GetHealth(HealAmount, gameObject);
            }
        }
    }
}
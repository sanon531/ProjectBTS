using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class BulletTeleportManager : MMSingleton<BulletTeleportManager>
    {
        public LinkedList<GameObject> BulletStack = new LinkedList<GameObject>();

        [SerializeField] private bool UseTargetBulletIcon = false;

        [SerializeField] private GameObject TargetIcon;
        [Range(0, 2)]
        [SerializeField] private float TargetIconSize = 1f;

        

        protected override void Awake()
        {
            base.Awake();

            if (UseTargetBulletIcon)
            {
                TargetIcon.transform.localScale *= TargetIconSize;
            }
            
        }

        private void Update()
        {
            if (UseTargetBulletIcon)
            {
                ResetTargetIcon();
            }
        }

        public void AddBullet(GameObject targetBullet)
        {
            BulletStack.AddLast(targetBullet);
        }


        public void DeleteBullet(GameObject targetBullet)
        {
            BulletStack.Remove(targetBullet);
        }

        private void ResetTargetIcon()
        {
            if (BulletStack.Last != null)
            {
                TargetIcon.SetActive(true);
                TargetIcon.transform.position = BulletStack.Last.Value.transform.position;
            }
            else
            {
                TargetIcon.SetActive(false);
            }
        }

        public void printStack()
        {
            foreach (GameObject i in BulletStack)
            {
                Debug.Log(i.transform.position);
            }
            Debug.Log("----------------------");
        }
        
    }
}
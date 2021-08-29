using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class BulletTeleportManager : MMSingleton<BulletTeleportManager>
    {
        public LinkedList<GameObject> BulletStack;

        [SerializeField] private bool UseTargetBulletIcon = false;
        [SerializeField] private GameObject TargetIcon;
        [Range(0, 2)]
        [SerializeField] private float TargetIconSize = 1f;

        [Header("Border")]
        public Transform leftDown;
        public Transform rightUp;


        protected override void Awake()
        {
            base.Awake();
            BulletStack = new LinkedList<GameObject>();

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
            BulletStack.Remove(BulletStack.Find(targetBullet));
        }

        public bool isContain(GameObject targetBullet)
        {
            return BulletStack.Contains(targetBullet);
        }

        private void ResetTargetIcon()
        {
            if (BulletStack.Count != 0)
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
                Debug.Log(i);
            }
            Debug.Log("----------------------");
        }
        
    }
}
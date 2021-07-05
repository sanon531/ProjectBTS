using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class BulletTeleportManager : MMSingleton<BulletTeleportManager>
    {
        public LinkedList<GameObject> BulletStack = new LinkedList<GameObject>();
        
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
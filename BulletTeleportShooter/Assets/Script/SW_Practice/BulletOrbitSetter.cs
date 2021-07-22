﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class BulletOrbitSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    MMAutoRotate autoRotate;
    void Start()
    {
        autoRotate = GetComponent<MMAutoRotate>();
        autoRotate.OrbitCenterTransform = PlayerOrbitSetter.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
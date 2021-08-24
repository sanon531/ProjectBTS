using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;

public class ToMousePos : MonoBehaviour
{

    public MMUIFollowMouse mMUIFollowMouse;
    
    void Start()
    {
        mMUIFollowMouse = GetComponent<MMUIFollowMouse>();
        mMUIFollowMouse.TargetCanvas = GUIManager.Instance.MainCanvas;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
    public class ShootAimJoyStick : MMTouchJoystick
    {
        public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);
			InputManager.Instance.ShootButtonDown();
			Invoke("ShootStop", 0.001f);
		}

		private void ShootStop()
        {
			InputManager.Instance.ShootButtonUp();
		}
	}
}
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
    public class ShootAimJoyStick : MMTouchJoystick
    {
		public enum ShootAimJoystickModes { OnDrag, EndDrag }
		
		[Header("BTS Joystick Mode")]
		public ShootAimJoystickModes ShootAimJoystickMod;

		public override void OnDrag(PointerEventData eventData)
        {
			base.OnDrag(eventData);

			if (ShootAimJoystickMod == ShootAimJoystickModes.OnDrag)
            {
				ShootStart();
			}
        }

		public override void OnEndDrag(PointerEventData eventData)
		{
			base.OnEndDrag(eventData);

			if (ShootAimJoystickMod == ShootAimJoystickModes.OnDrag)
            {
				ShootStop();
			}

			else if (ShootAimJoystickMod == ShootAimJoystickModes.EndDrag)
            {
				ShootStart();
				Invoke("ShootStop", 0.001f);
			}
		}

		private void ShootStart()
        {
			InputManager.Instance.ShootButtonDown();
		}

		private void ShootStop()
        {
			InputManager.Instance.ShootButtonUp();
		}
	}
}
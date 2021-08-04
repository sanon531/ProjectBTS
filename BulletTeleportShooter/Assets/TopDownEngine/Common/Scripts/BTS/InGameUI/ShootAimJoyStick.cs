using MoreMountains.TopDownEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
    public class ShootAimJoyStick : MMTouchJoystick
    {
		public enum ShootAimJoystickModes { OnDrag, EndDrag }
		
		[Header("BTS Joystick Mode")]
		public ShootAimJoystickModes ShootAimJoystickMod;

		public override void OnDragKnob(PointerEventData eventData)
        {
			base.OnDragKnob(eventData);

			if (ShootAimJoystickMod == ShootAimJoystickModes.OnDrag)
            {
				ShootStart();
			}
        }

		public override void OnEndDragKnob(PointerEventData eventData)
		{
			base.OnEndDragKnob(eventData);

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
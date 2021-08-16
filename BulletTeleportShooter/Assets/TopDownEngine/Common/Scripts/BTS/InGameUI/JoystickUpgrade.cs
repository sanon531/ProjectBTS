using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
    public class JoystickUpgrade : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private MMTouchJoystick _mmTouchJoystick;
        private ShootAimJoyStick _shootAimJoyStick;

        void Start()
        {
            _mmTouchJoystick = GetComponentInChildren<MMTouchJoystick>();
            _shootAimJoyStick = GetComponentInChildren<ShootAimJoyStick>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_shootAimJoyStick == null)
            {
                _mmTouchJoystick.OnDragKnob(eventData);
            }

            else
            {
                _shootAimJoyStick.OnDragKnob(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_shootAimJoyStick == null)
            {
                _mmTouchJoystick.OnEndDragKnob(eventData);
            }

            else
            {
                _shootAimJoyStick.OnEndDragKnob(eventData);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_shootAimJoyStick == null)
            {
                _mmTouchJoystick.OnDragKnob(eventData);
            }

            else
            {
                _shootAimJoyStick.OnDragKnob(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_shootAimJoyStick == null)
            {
                _mmTouchJoystick.OnEndDragKnob(eventData);
            }

            else
            {
                _shootAimJoyStick.OnEndDragKnob(eventData);
            }
        }
    }
}
﻿using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    public class OptionSystem : MonoBehaviour
    {
        [SerializeField] private Text HandUIText;
        [SerializeField] private GameObject LockIcon;

        private enum UIHandStates { RightHand, LeftHand }
        private UIHandStates UIHandState;
        private MMSoundManager SoundManager;
        private RectTransform MoveJoystickTr;
        private RectTransform ShootJoystickTr;
        private RectTransform TeleportBtTr;

        private void Awake()
        {
            SoundManager = MMSoundManager.Instance;
            UIHandState = UIHandStates.RightHand;

            if (GUIManager.Instance.Joystick != null)
            {
                MoveJoystickTr = GUIManager.Instance.Joystick.GetComponent<RectTransform>();
            }
            if (GUIManager.Instance.SecondJoystick != null)
            {
                ShootJoystickTr = GUIManager.Instance.SecondJoystick.GetComponent<RectTransform>();
            }   
            if (GUIManager.Instance.Buttons != null)
            {
                TeleportBtTr = GUIManager.Instance.Buttons.GetComponentInChildren<RectTransform>();
            }
        }

        private void Start()
        {
            if (InputManager.Instance.InputForcedMode == InputManager.InputForcedModes.Desktop)
            {
                HandUIText.text = "";
                LockIcon.SetActive(true);
            }
        }

        public void SetVolume(float volume)
        {
            SoundManager.SetVolumeMaster(volume * 2);
        }

        public void UIMoveButton()
        {
            if (InputManager.Instance.InputForcedMode == InputManager.InputForcedModes.Mobile)
            {
                if (UIHandState == UIHandStates.RightHand)
                {
                    UIHandState = UIHandStates.LeftHand;
                    HandUIText.text = "Left Hand";
                    ChangeJoystickPos();
                }
                else
                {
                    UIHandState = UIHandStates.RightHand;
                    HandUIText.text = "Right Hand";
                    ChangeJoystickPos();
                }
            }
        }

        private void ChangeJoystickPos()
        {
            if (MoveJoystickTr != null)
            {
                MoveJoystickTr.anchoredPosition = new Vector2(-MoveJoystickTr.anchoredPosition.x, MoveJoystickTr.anchoredPosition.y);
                MoveJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();
            }

            if (ShootJoystickTr != null)
            {
                ShootJoystickTr.anchoredPosition = new Vector2(-ShootJoystickTr.anchoredPosition.x, ShootJoystickTr.anchoredPosition.y);
                ShootJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();
            }    

            if (TeleportBtTr != null)
            {
                TeleportBtTr.anchoredPosition = new Vector2(-TeleportBtTr.anchoredPosition.x, TeleportBtTr.anchoredPosition.y);
            }
        }
    }
}
using MoreMountains.Tools;
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
        [SerializeField] private Slider SoundSlider;

        private enum UIHandStates { RightHand, LeftHand }
        private static UIHandStates UIHandState;
        private MMSoundManager SoundManager;
        private RectTransform MoveJoystickTr;
        private RectTransform ShootJoystickTr;
        private RectTransform TeleportBtTr;

        private void Awake()
        {
            SoundManager = MMSoundManager.Instance;
        }

        private void Start()
        {
            if (InputManager.Instance.InputForcedMode == InputManager.InputForcedModes.Desktop)
            {
                HandUIText.text = "";
                LockIcon.SetActive(true);
            }

            SoundSlider.value = SoundManager.settingsSo.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master);

            /* 
            if (세이브파일 == 오른손)
            {
                UIHandState = UIHandStates.RightHand;
            }
            else if (세이브파일 == 왼손)
            {
                UIHandState = UIHandStates.LeftHand;
            }
            */

            SetInitialJoystickUI();
        }

        public void SetVolume(float volume)
        {
            SoundManager.SetVolumeMaster(volume);
        }

        public void UIMoveButton()
        {
            if (InputManager.Instance.InputForcedMode == InputManager.InputForcedModes.Mobile)
            {
                if (UIHandState == UIHandStates.RightHand)
                {
                    UIHandState = UIHandStates.LeftHand;

                    /*
                    세이브파일 -> 왼손으로 변경
                    */
                }
                else
                {
                    UIHandState = UIHandStates.RightHand;

                    /*
                    세이브파일 -> 오른손으로 변경
                    */             
                }

                SetJoystickUI();
            }
        }

        public void SetInitialJoystickUI()
        {
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
                TeleportBtTr = GUIManager.Instance.Buttons.transform.GetComponentInChildren<RectTransform>();
            }

            if (InputManager.Instance.InputForcedMode == InputManager.InputForcedModes.Mobile)
            {
                SetJoystickUI();
            }
        }

        private void SetJoystickUI()
        {          
            if (UIHandState == UIHandStates.RightHand)
            {
                HandUIText.text = "Right Hand";

                if (MoveJoystickTr != null && MoveJoystickTr.anchoredPosition.x > 0)
                {
                    MoveJoystickTr.anchoredPosition = new Vector2(-MoveJoystickTr.anchoredPosition.x, MoveJoystickTr.anchoredPosition.y);
                    MoveJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();
                }

                if (ShootJoystickTr != null && MoveJoystickTr != ShootJoystickTr && ShootJoystickTr.anchoredPosition.x > 0)
                {
                    ShootJoystickTr.anchoredPosition = new Vector2(-ShootJoystickTr.anchoredPosition.x, ShootJoystickTr.anchoredPosition.y);
                    ShootJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();
                }

                if (TeleportBtTr != null && TeleportBtTr.anchoredPosition.x < 0)
                {
                    TeleportBtTr.anchoredPosition = new Vector2(-TeleportBtTr.anchoredPosition.x, TeleportBtTr.anchoredPosition.y);
                }
            }

            else
            {
                HandUIText.text = "Left Hand";

                if (MoveJoystickTr != null && MoveJoystickTr.anchoredPosition.x < 0)
                {
                    MoveJoystickTr.anchoredPosition = new Vector2(-MoveJoystickTr.anchoredPosition.x, MoveJoystickTr.anchoredPosition.y);
                    MoveJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();    
                }

                if (ShootJoystickTr != null && MoveJoystickTr != ShootJoystickTr && ShootJoystickTr.anchoredPosition.x < 0)
                {
                    ShootJoystickTr.anchoredPosition = new Vector2(-ShootJoystickTr.anchoredPosition.x, ShootJoystickTr.anchoredPosition.y);
                    ShootJoystickTr.GetComponentInChildren<MMTouchJoystick>().Initialize();
                }

                if (TeleportBtTr != null && TeleportBtTr.anchoredPosition.x > 0)
                {
                    TeleportBtTr.anchoredPosition = new Vector2(-TeleportBtTr.anchoredPosition.x, TeleportBtTr.anchoredPosition.y);
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    public class OptionButton : MonoBehaviour
    {
        public void OptionButtonAction()
        {
            GUIManager.Instance.OptionScreen.SetActive(true);
        }
        public void OptionClose()
        {
            GUIManager.Instance.OptionScreen.SetActive(false);
        }
        public void PauseReopen()
        {
            GUIManager.Instance.PauseScreen.SetActive(true);
        }
        public void PauseTempClose()
        {
            GUIManager.Instance.PauseScreen.SetActive(false);
        }
    }
}
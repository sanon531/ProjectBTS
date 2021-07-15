using MoreMountains.Tools;
using UnityEngine.UI;


namespace MoreMountains.TopDownEngine
{
    public class OverloadTextManager : MMSingleton<OverloadTextManager>
    {
        public Text OverloadText;

        public void SetOverloadText(int nowOverload)
        {
            if (OverloadText != null)
            {
                OverloadText.text = nowOverload.ToString("000");
            }
        }
    }
}
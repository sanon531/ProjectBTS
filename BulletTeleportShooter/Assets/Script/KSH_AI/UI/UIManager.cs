using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } = null;

    [SerializeField] private UI_Title ui_Title;
    [SerializeField] private UI_Notice ui_Notice;

    private void Awake()
    {
        Instance = this;
    }

    public void MakeTitle(string _content, float _time)
    {
        ui_Title.MakeTitle(_content, _time);
    }

    public void MakeNotice(string _content, float _time)
    {
        ui_Notice.MakeNotice(_content, _time);
    }
}

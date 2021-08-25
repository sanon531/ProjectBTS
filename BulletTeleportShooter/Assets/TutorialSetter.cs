using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    ButtonFunctions functions;


    void Start()
    {
        if (GetComponent<Button>() != null)
            GetComponent<Button>().onClick.AddListener(() => OnClick());
    }
    public void OnClick()
    {
        if (SaveAndLoad.instance.saveData.tutorialOn)
        {
            SaveAndLoad.instance.SetTutorial(false);
            functions.OnClick();
            Debug.Log("asdfasdf d으아 튜토리얼 가즈아ㅏㅏ");
        }


    }



}

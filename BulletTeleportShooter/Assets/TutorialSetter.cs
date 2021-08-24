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
        SaveAndLoad.instance.SetTutorial(false);
    }



}

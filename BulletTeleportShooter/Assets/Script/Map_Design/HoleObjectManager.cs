using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleObjectManager : MonoBehaviour
{
    public static HoleObjectManager Instance;

    [SerializeField]
    List<BTS_HoleObjectContainer> HoleContainerList = new List<BTS_HoleObjectContainer>();

    


    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public float ti = 0;
    // Update is called once per frame
    void Update()
    {
        ti += Time.deltaTime;

        if (ti >= 3f)
        {
            Destroy(BTS_HoleObjectContainer.FirstContainer.gameObject);
            //Timer Timer_GameTime = GameObject.Find("GameManager").GetComponent<Timer>();
        }
        else if(ti>=6f)
        {
            Destroy(BTS_HoleObjectContainer.SecondContainer.gameObject);

        }
    }
}

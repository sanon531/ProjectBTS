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
    private void Start()
    {
        Destroy(BTS_HoleObjectContainer.FirstContainer.gameObject, 30f);
        Destroy(BTS_HoleObjectContainer.SecondContainer.gameObject, 60f);
        Destroy(BTS_HoleObjectContainer.ThirdContainer.gameObject, 90f);
    }
    // Update is called once per frame
    void Update()
    {

        



    }
}

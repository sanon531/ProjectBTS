using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTS_HoleObjectContainer : MonoBehaviour
{
    public static BTS_HoleObjectContainer FirstContainer;
    public static BTS_HoleObjectContainer SecondContainer;
    public static BTS_HoleObjectContainer ThirdContainer;

    public int number = 0;

    [SerializeField]
    List<BTS_HoleObjectData> HoleList = new List<BTS_HoleObjectData>();

    public void AddHole(BTS_HoleObjectData data)
    {
        HoleList.Add(data);
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (number == 0)
        {
            FirstContainer = this;
        }
        else if (number == 1)
        {
            SecondContainer = this;
        }
        else
        {
            ThirdContainer = this;
        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

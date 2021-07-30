using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{

    public Area[] accessableArea;

    public bool CheckAreaIsAccessable(Area _area)
    {
        if (_area == this) return true;
        for(int i = 0; i < accessableArea.Length; ++i)
        {
            if (accessableArea[i] == _area) return true;
        }
        return false;
    }
}

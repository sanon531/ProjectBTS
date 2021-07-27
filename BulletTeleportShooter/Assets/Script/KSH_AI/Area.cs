using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public class Border
    {
        public Area rootArea;
        public Area targetArea;
        private float constValue;
        private Vector2 areaValue;
        private bool isXLine;

        public Border(Area _rootArea, Area _targetArea, float _constValue, Vector2 _areaValue, bool _isXLine)
        {
            rootArea = _rootArea;
            targetArea = _targetArea;
            constValue = _constValue;
            areaValue = _areaValue;
            isXLine = _isXLine;
        }

        public PathData GetMinPathData(Vector2 _pos)
        {
            if (isXLine)
            {
                if (_pos.y < areaValue.x)
                {
                    Vector2 destination = new Vector2(constValue, areaValue.x);
                    return new PathData(destination, (_pos - destination).sqrMagnitude);
                }
                else if (areaValue.x <= _pos.y && _pos.y <= areaValue.y)
                {
                    return new PathData(new Vector2(constValue, _pos.y), Mathf.Pow(constValue - _pos.x, 2));
                }
                else
                {
                    Vector2 destination = new Vector2(constValue, areaValue.y);
                    return new PathData(destination, (_pos - destination).sqrMagnitude);
                }
            }
            else
            {
                if (_pos.x < areaValue.x)
                {
                    Vector2 destination = new Vector2(areaValue.x, constValue);
                    return new PathData(destination, (_pos - destination).sqrMagnitude);
                }
                else if (areaValue.x <= _pos.x && _pos.x <= areaValue.y)
                {
                    return new PathData(new Vector2(_pos.x, constValue), Mathf.Pow(constValue - _pos.y, 2));
                }
                else
                {
                    Vector2 destination = new Vector2(areaValue.y, constValue);
                    return new PathData(destination, (_pos - destination).sqrMagnitude);
                }
            }
        }
    }


    public Area[] accessableArea;


    //border는 x = a, y = b 식으로 나타내어 질 수 있는 직선이어야 한다.
    public Dictionary<Area, List<NewPathManager.NewNode>> borderNodeData;

    public Dictionary<Area, Border> borderData;

    public void Init()
    {
        borderNodeData = new Dictionary<Area, List<NewPathManager.NewNode>>(accessableArea.Length);
        for(int i = 0; i < accessableArea.Length; ++i)
        {
            borderNodeData.Add(accessableArea[i], new List<NewPathManager.NewNode>());
        }
    }

    public void BuildBorderData()
    {
        borderData = new Dictionary<Area, Border>();
        foreach(KeyValuePair<Area, List<NewPathManager.NewNode>> kv in borderNodeData)
        {
            if(kv.Value != null && kv.Value.Count > 0)
            {
                NewPathManager.NewNode firstNode = kv.Value[0];
                NewPathManager.NewNode lastNode = kv.Value[kv.Value.Count - 1];
                bool isXLine = firstNode.pos.x == lastNode.pos.x;
                if (kv.Value.Count > 1)
                {
                    if (isXLine)
                    {
                        kv.Value.Sort((x1, x2) => x1.pos.x.CompareTo(x2.pos.x));
                    }
                    else
                    {
                        kv.Value.Sort((x1, x2) => x1.pos.y.CompareTo(x2.pos.y));
                    }
                }
                firstNode = kv.Value[0];
                lastNode = kv.Value[kv.Value.Count - 1];
                float constValue;
                Vector2 areaValue;
                if (isXLine)
                {
                    constValue = firstNode.pos.x;
                    areaValue = new Vector2(firstNode.pos.y, lastNode.pos.y);
                }
                else
                {
                    constValue = firstNode.pos.y;
                    areaValue = new Vector2(firstNode.pos.x, lastNode.pos.x);
                }
                borderData.Add(kv.Key, new Border(this, kv.Key, constValue, areaValue, isXLine));
            }
        }
    }



    

}

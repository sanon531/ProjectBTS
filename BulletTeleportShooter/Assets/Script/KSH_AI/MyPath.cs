using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyPath : MonoBehaviour
{
    public float cost;
    private Vector3[] paths;
    private Color color;
    bool isInit = false;

    public MyPath SetPath(Vector3[] _path, float _cost, Color _color)
    {
        cost = _cost;
        paths = _path;
        color = _color;
        isInit = true;
        return this;
    }

    public MyPath SetActive(bool _state)
    {
        gameObject.SetActive(_state);
        return this;
    }

    public MyPath SetName(string _content)
    {
        name = _content;
        return this;
    }

    private void OnDrawGizmosSelected()
    {
        if (isInit && PathManager.Instance.IsDebugMode)
        {
            Gizmos.color = color;
            for(int i = 1; i < paths.Length; ++i)
            {
                Gizmos.DrawLine(paths[i - 1], paths[i]);
            }
        }
    }

}

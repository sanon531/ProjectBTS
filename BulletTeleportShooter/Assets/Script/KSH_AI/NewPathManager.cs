using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<T>
{
    public T Data { get; set; }
    public List<TreeNode<T>> child;

    public TreeNode(T _data)
    {
        Data = _data;
        child = new List<TreeNode<T>>();
    }

    public void AddItem(T _data)
    {
        child.Add(new TreeNode<T>(_data));
    }

    public TreeNode<T> GetItem(int _index)
    {
        return child[_index];
    }
}

public class Tree<T> where T : class
{
    public TreeNode<T> Root { get; set; }

    public Tree(T _root = null)
    {
        Root = new TreeNode<T>(_root);
    }
}

public class PathData
{
    public Vector2 destination;
    public float distance;

    public PathData(Vector2 _des, float _dis)
    {
        destination = _des;
        distance = _dis;
    }
}

public class NewPathManager : MonoBehaviour
{
    [Serializable]
    public class AreaList
    {
        public Area[] area;
    }

    public static NewPathManager Instance { get; private set; } = null;
    private Dictionary<Vector2, Area> mapData;

    [SerializeField] private Grid grid;
    [Header("- Border")]
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightUp;
    [Header("- Area Data")]
    [SerializeField] private AreaList[] areaData;
    public class NewNode
    {
        public Vector2 pos;
        public Area area;
    }


    private void Awake()
    {
        Instance = this;
    }

    private Dictionary<Vector2, Area> MakeMapData()
    {
        Vector2Int leftDownPos = ((Vector2Int)grid.WorldToCell(leftDown.position));
        Vector2Int rightUpPos = ((Vector2Int)grid.WorldToCell(rightUp.position));
        Vector2 cellSize = new Vector2(grid.cellSize.x, grid.cellSize.y);
        Vector2 mapSize = new Vector2((rightUpPos.x - leftDownPos.x) / cellSize.x, (rightUpPos.y - leftDownPos.y) / cellSize.y);

        Dictionary<Vector2, Area> newMapData = new Dictionary<Vector2, Area>((int)(mapSize.x * mapSize.y));
        List<List<NewNode>> newNodeData = new List<List<NewNode>>((int)(mapSize.x * mapSize.y));

        for (int x = 0; x < mapSize.x; ++x)
        {
            newNodeData[x] = new List<NewNode>();
            for(int y = 0; y < mapSize.y; ++y)
            {
                Vector2 cellPos = new Vector2(leftDownPos.x + cellSize.x * x, leftDownPos.y + cellSize.y * y);
                RaycastHit2D[] hit2D = Physics2D.BoxCastAll(new Vector2(cellPos.x + cellSize.x * .5f, cellPos.y + cellSize.y * .5f), new Vector2(0.95f, 0.95f), 0, Vector2.zero);
                NewNode newNode = new NewNode();
                newNode.pos = cellPos;
                for(int i = 0; i < hit2D.Length; ++i)
                {
                    Area area;
                    if(hit2D[i].transform.TryGetComponent(out area))
                    {
                        newNode.area = area;
                        newMapData.Add(cellPos, area);
                        newNodeData[x][y] = newNode;
                    }
                }
            }
        }

        // Border 정보 등록
        for(int x = 0; x < mapSize.x; ++x)
        {
            for(int y = 0; y < mapSize.y; ++y)
            {
                NewNode currentNode = newNodeData[x][y];
                for(int _x = -1; _x == 1; ++_x)
                {
                    for(int _y = -1; _y == 1; ++_y)
                    {
                        if(0 <= x + _x && x + _x < mapSize.x && 0 <= y + _y && y + _y < mapSize.y)
                        {
                            NewNode compareNode = newNodeData[x + _x][y + _y];
                            if(currentNode.area != compareNode.area)
                            {
                                for(int i = 0; i < currentNode.area.accessableArea.Length; ++i)
                                {
                                    if(currentNode.area.accessableArea[i] == compareNode.area)
                                    {
                                        if(!currentNode.area.borderNodeData[currentNode.area.accessableArea[i]].Contains(currentNode))
                                            currentNode.area.borderNodeData[currentNode.area.accessableArea[i]].Add(currentNode);
                                        if(!compareNode.area.borderNodeData[currentNode.area].Contains(compareNode))
                                            compareNode.area.borderNodeData[currentNode.area].Add(compareNode);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return newMapData;
    }

    private Area GetMapData(Vector2 _pos)
    {
        return mapData[(Vector2Int)grid.WorldToCell(_pos)];
    }


    public List<Vector3> FindPath(Vector3 _currentPos, Vector3 _desPos)
    {
        Area startArea = mapData[_currentPos];
        Area endArea = mapData[_desPos];
        List<Vector3> path = new List<Vector3>();
        if(startArea == endArea)
        {
            // 같은 Area에 있다 = 직선으로 이동 가능하다
            path.Add(_desPos);
            return path;
        }

        if (false)
        {
            // 여기서 갈 수 없음을 판정
            return null;
        }

        Tree<PathData> tree = new Tree<PathData>();
        Area prevArea = null;
        Area currentArea = startArea;
        TreeNode<PathData> currentNode = tree.Root;
        Vector3 currentPos = _currentPos;
        for(int pathIndex = 0; pathIndex < startArea.accessableArea.Length; ++pathIndex)
        {
            PathData pathData = currentArea.borderData[currentArea.accessableArea[pathIndex]].GetMinPathData(currentPos);
            currentNode.AddItem(pathData);
        }

/*
        for (int i = 0; i < areaData[0].area.Length; ++i) // 이 루프에서 i의 의미는 딱히 없다. 루프를 areaData[]의 크기만큼 돌리는 것 뿐.
        {
            for(int j = 0; j < currentArea.accessableArea.Length; ++j)
            {
                if (currentArea.accessableArea[j] == prevArea) continue;

                currentNode.AddItem(currentArea.borderData[currentArea.accessableArea[j]].GetMinPathData(currentPos));

            }

            for(int pathIndex = 0; pathIndex < currentNode.child.Count; ++pathIndex)
            {
                TreeNode<PathData> newNode = currentNode.GetItem(pathIndex);
                Area newArea = GetMapData(newNode.Data.destination);
                
                for(int areaIndex = 0; areaIndex < newArea.accessableArea.Length; ++areaIndex)
                {
                    if (newArea.accessableArea[areaIndex] == currentArea) continue;
                    newNode.AddItem(newArea.borderData[newArea.accessableArea[areaIndex]].GetMinPathData(currentNode.Data.destination));

                }
            }
*/


            prevArea = currentArea; // 역행을 방지
            return null;
        }
    }

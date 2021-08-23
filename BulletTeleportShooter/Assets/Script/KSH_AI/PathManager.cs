using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreeNode<T>
{
    public T Data { get; set; }
    public TreeNode<T> root;
    public List<TreeNode<T>> child;

    public TreeNode(T _data)
    {
        Data = _data;
        root = null;
        child = new List<TreeNode<T>>();
    }

    public TreeNode<T> AddItem(T _data)
    {
        TreeNode<T> newItem = new TreeNode<T>(_data);
        newItem.root = this;
        child.Add(newItem);
        return newItem;
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
    public Area desArea;
    public HashSet<Area> pathArea; // 이제까지 지나온 Area

    public PathData(Vector2 _des, float _dis, Area _area)
    {
        destination = _des;
        distance = _dis;
        desArea = _area;
        pathArea = new HashSet<Area>();
    }
}

public class Border
{
    public Area rootArea;
    public Area targetArea;
    public float constValue;
    public Vector2 areaValue;
    public bool IsXLine { get; private set; }

    public Vector2 StartPos
    {
        get
        {
            return IsXLine ? new Vector2(constValue, areaValue.x) : new Vector2(areaValue.x, constValue);
        }
    }

    public Vector2 EndPos
    {
        get
        {
            return IsXLine ? new Vector2(constValue, areaValue.y) : new Vector2(areaValue.y, constValue);
        }
    }

    public Border(Area _rootArea, Area _targetArea, float _constValue, Vector2 _areaValue, bool _isXLine)
    {
        rootArea = _rootArea;
        targetArea = _targetArea;
        constValue = _constValue;
        areaValue = _areaValue;
        IsXLine = _isXLine;
    }

    public PathData GetMinPathData(Vector2 _pos)
    {
        if (IsXLine)
        {
            if (_pos.y < areaValue.x)
            {
                Vector2 destination = new Vector2(constValue, areaValue.x);
                return new PathData(destination, (_pos - destination).sqrMagnitude, targetArea);
            }
            else if (areaValue.x <= _pos.y && _pos.y <= areaValue.y)
            {
                return new PathData(new Vector2(constValue, _pos.y), Mathf.Pow(constValue - _pos.x, 2), targetArea);
            }
            else
            {
                Vector2 destination = new Vector2(constValue, areaValue.y);
                return new PathData(destination, (_pos - destination).sqrMagnitude, targetArea);
            }
        }
        else
        {
            if (_pos.x < areaValue.x)
            {
                Vector2 destination = new Vector2(areaValue.x, constValue);
                return new PathData(destination, (_pos - destination).sqrMagnitude, targetArea);
            }
            else if (areaValue.x <= _pos.x && _pos.x <= areaValue.y)
            {
                return new PathData(new Vector2(_pos.x, constValue), Mathf.Pow(constValue - _pos.y, 2), targetArea);
            }
            else
            {
                Vector2 destination = new Vector2(areaValue.y, constValue);
                return new PathData(destination, (_pos - destination).sqrMagnitude, targetArea);
            }
        }
    }
}

public class Node
{
    public Vector2 pos;
    public Area area;
}

public class NodeComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        if (x.pos.x.CompareTo(y.pos.x) != 0)
        {
            return x.pos.x.CompareTo(y.pos.x);
        }
        else if(x.pos.y.CompareTo(y.pos.y) != 0)
        {
            return x.pos.y.CompareTo(y.pos.y);
        }
        else
        {
            return 0;
        }
    }
}

public class PathManager : MonoBehaviour
{
    //AreaIndex는 AreaList의 집합입니다. 지형의 변동 등으로 인하여 구역이 변동될경우, AreaIndex를 교체합니다.
    [Serializable]
    public class AreaIndex
    {
        public AreaList[] area;
    }

    //AreaList는 Area의 집합입니다. AreaList 안에 들어 있는 area끼리는 서로 통행이 가능해야 합니다. 이 말은, AreaList 안에 없는 area는 통행이 블가능하다는 것입니다.
    [Serializable]
    public class AreaList
    {
        public Area[] area;
    }

    public static PathManager Instance { get; private set; } = null;
    private Dictionary<Vector2, Area> mapData;
    private Dictionary<Area, List<Border>> borderData;
    private List<List<Node>> nodeData;
    private Dictionary<Area, string> indexData;
    private Dictionary<Area, int> areaIndexData;        // 같은 index의 Area끼리는 통행이 가능하다 (다르면 불가능하다)
    private Dictionary<Area, int> areaMaxIndexData;     // Area가 통행 가능한 최대 Area의 수
    [SerializeField] public bool IsDebugMode = false;

    [SerializeField] private MyPath pathDrawer;
    [SerializeField] private Transform pathDebugger;
    [SerializeField] private Transform borderDebugger;
    [SerializeField] private Grid grid;
    [Header("- Border")]
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightUp;
    [Header("- Area Data")]
    [SerializeField] private Transform areaData;
    [SerializeField] private AreaIndex[] areaIndex;

    private void Awake()
    {
        Instance = this;
        if (areaIndex.Length <= 0) {
            areaIndex = new AreaIndex[areaData.childCount];
            for (int i = 0; i < areaIndex.Length; ++i)
            {
                areaIndex[i] = new AreaIndex();
                areaIndex[i].area = new AreaList[areaData.GetChild(i).childCount];
                for (int j = 0; j < areaIndex[i].area.Length; ++j)
                {
                    areaIndex[i].area[j] = new AreaList();
                    areaIndex[i].area[j].area = new Area[areaData.GetChild(i).GetChild(j).childCount];
                    for (int k = 0; k < areaIndex[i].area[j].area.Length; ++k)
                    {
                        areaIndex[i].area[j].area[k] = areaData.GetChild(i).GetChild(j).GetChild(k).GetComponent<Area>();
                    }
                }
            }
        }
        indexData = new Dictionary<Area, string>();
        for (int i = 0; i < areaIndex.Length; ++i)
        {
            for (int j = 0; j < areaIndex[i].area.Length; ++j)
            {
                for (int k = 0; k < areaIndex[i].area[j].area.Length; ++k)
                {
                    indexData.Add(areaIndex[i].area[j].area[k], j > 0 ? "X" : $"{k}");
                }
            }
        }
    }

    private void Start()
    {
        MakeMapData(0);
    }

    public void MakeMapData(int _index)
    {
        if(0 <= _index && _index < areaIndex.Length)
        {
            MakeMapData(areaIndex[_index]);
        }
    }

    private void MakeMapData(AreaIndex _areaData)
    {
        Vector2Int leftDownPos = ((Vector2Int)grid.WorldToCell(leftDown.position));
        Vector2Int rightUpPos = ((Vector2Int)grid.WorldToCell(rightUp.position));
        Vector2 cellSize = new Vector2(grid.cellSize.x, grid.cellSize.y);
        Vector2 mapSize = new Vector2((rightUpPos.x - leftDownPos.x) / cellSize.x, (rightUpPos.y - leftDownPos.y) / cellSize.y);

        // Area 정보 등록
        areaIndexData = new Dictionary<Area, int>();
        areaMaxIndexData = new Dictionary<Area, int>();
        Dictionary<Area, Dictionary<Area, SortedSet<Node>>> borderNodeData = new Dictionary<Area, Dictionary<Area, SortedSet<Node>>>();
        for (int i = 0; i < _areaData.area.Length; ++i)
        {
            AreaList areaList = _areaData.area[i];
            for (int j = 0; j < areaList.area.Length; ++j)
            {
                Area area = areaList.area[j];

                areaIndexData.Add(area, i);
                areaMaxIndexData.Add(area, areaList.area.Length);

                Dictionary<Area, SortedSet<Node>> newBorderNodeData = new Dictionary<Area, SortedSet<Node>>();
                for (int areaIndex = 0; areaIndex < area.accessableArea.Length; ++areaIndex)
                {
                    Area accessableArea = area.accessableArea[areaIndex];
                    newBorderNodeData.Add(accessableArea, new SortedSet<Node>(new NodeComparer()));
                }
                borderNodeData.Add(area, newBorderNodeData);
            }
        }

        // 각 좌표가 어느 Area인지 정보 등록
        mapData = new Dictionary<Vector2, Area>((int)(mapSize.x * mapSize.y));
        nodeData = new List<List<Node>>((int)mapSize.x);
        for (int x = 0; x < mapSize.x; ++x)
        {
            nodeData.Add(new List<Node>((int)mapSize.y));
            for (int y = 0; y < mapSize.y; ++y)
            {
                Vector2 cellPos = new Vector2(leftDownPos.x + cellSize.x * x, leftDownPos.y + cellSize.y * y);
                Vector2 cellMidPos = new Vector2(cellPos.x + cellSize.x * .5f, cellPos.y + cellSize.y * .5f);
                RaycastHit2D[] hit2D = Physics2D.BoxCastAll(cellMidPos, new Vector2(0.95f, 0.95f), 0, Vector2.zero, 0, LayerMask.GetMask("Area"));
                Node newNode = new Node();
                newNode.pos = cellMidPos;
                for (int i = 0; i < hit2D.Length; ++i)
                {
                    Area area;
                    if (hit2D[i].transform.TryGetComponent(out area))
                    {
                        newNode.area = area;
                        mapData.Add(cellPos, area);
                        nodeData[x].Add(newNode);
                    }
                }
            }
        }

        // 각 좌표에 저장된 Area를 기반으로 Border 계산
        for (int x = 0; x < mapSize.x; ++x)
        {
            for (int y = 0; y < mapSize.y; ++y)
            {
                Node currentNode = nodeData[x][y];
                for (int _x = -1; _x <= 1; ++_x)
                {
                    for (int _y = -1; _y <= 1; ++_y)
                    {
                        if (0 <= x + _x && x + _x < mapSize.x && 0 <= y + _y && y + _y < mapSize.y)
                        {
                            Node compareNode = nodeData[x + _x][y + _y];
                            if (currentNode.area != compareNode.area && currentNode.area.CheckAreaIsAccessable(compareNode.area))
                            {

                                borderNodeData[currentNode.area][compareNode.area].Add(compareNode);
                            }
                        }
                    }
                }
            }
        }

        // 맞붙어 있는 Border끼리 합체
        borderData = new Dictionary<Area, List<Border>>();
        foreach (KeyValuePair<Area, Dictionary<Area, SortedSet<Node>>> kv in borderNodeData)
        {
            Area rootArea = kv.Key;
            List<Border> newBorderList = new List<Border>(borderNodeData[rootArea].Count);
            foreach (KeyValuePair<Area, SortedSet<Node>> _kv in kv.Value)
            {
                Area targetArea = _kv.Key;

                Node firstNode = _kv.Value.Min;
                Node lastNode = _kv.Value.Max;

                bool isXLine = firstNode.pos.x == lastNode.pos.x;
                float constValue = isXLine ? firstNode.pos.x : firstNode.pos.y;
                Vector2 areaValue = isXLine ? new Vector2(firstNode.pos.y, lastNode.pos.y) : new Vector2(firstNode.pos.x, lastNode.pos.x);

                newBorderList.Add(new Border(rootArea, targetArea, constValue, areaValue, isXLine));
            }
            borderData.Add(rootArea, newBorderList);
        }

        HashSet<Border> checkedBorder = new HashSet<Border>();
        foreach (KeyValuePair<Area, List<Border>> kv in borderData)
        {
            Area rootArea = kv.Key;
            for (int i = 0; i < kv.Value.Count; ++i)
            {
                if (checkedBorder.Contains(kv.Value[i])) continue;
                Border border = kv.Value[i];
                Border nextBorder = borderData[border.targetArea].Find(x => x.targetArea == rootArea);

                float midConstValue = (border.constValue + nextBorder.constValue) * .5f;
                Vector2 midAreaValue = new Vector2(Mathf.Max(border.areaValue.x, nextBorder.areaValue.x), Mathf.Min(border.areaValue.y, nextBorder.areaValue.y));

                border.constValue = midConstValue;
                nextBorder.constValue = midConstValue;

                border.areaValue = midAreaValue;
                nextBorder.areaValue = midAreaValue;

                checkedBorder.Add(border);
                checkedBorder.Add(nextBorder);
            }
        }

        // Border 디버깅
        foreach (KeyValuePair<Area, List<Border>> kv in borderData)
        {
            for (int i = 0; i < kv.Value.Count; ++i)
            {
                Instantiate(pathDrawer, borderDebugger).SetName("Border").SetPath(new Vector3[] { kv.Value[i].StartPos, kv.Value[i].EndPos }, 0, Color.black).SetActive(true);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!IsDebugMode) return;
        try
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 8;
            for (int x = 0; x < nodeData.Count; ++x)
            {
                for (int y = 0; y < nodeData[x].Count; ++y)
                {
                    //Handles.Label(nodeData[x][y].pos, indexData[nodeData[x][y].area], style);
                }
            }

        }
        catch (Exception)
        {

        }
    }

    private Area GetMapData(Vector2 _pos)
    {
        return mapData[(Vector2Int)grid.WorldToCell(_pos)];
    }

    public List<Vector3> FindPath(Vector3 _currentPos, Vector3 _desPos)
    {
        Area startArea = GetMapData(_currentPos);
        Area endArea = GetMapData(_desPos);
        List<Vector3> path = new List<Vector3>();
        if (startArea == endArea)
        {
            // 같은 Area에 있다 = 직선으로 이동 가능하다
            path.Add(_currentPos);
            path.Add(_desPos);
            return path;
        }

        if (areaIndexData[startArea] != areaIndexData[endArea])
        {
            return null;
        }

        bool isFirst = true;
        Tree<PathData> tree = new Tree<PathData>();
        List<TreeNode<PathData>> prevData = new List<TreeNode<PathData>>(); // 이전 계층
        HashSet<TreeNode<PathData>> endPathData = new HashSet<TreeNode<PathData>>(); // 완료된 노드들을 담아놓는 곳
        while (true)
        {
            List<TreeNode<PathData>> currentData = new List<TreeNode<PathData>>(); // 현재 계층
            if (isFirst)
            {
                tree.Root = new TreeNode<PathData>(new PathData(_currentPos, 0, startArea));
                TreeNode<PathData> prevNode = tree.Root;
                Area currentArea = startArea;
                Vector3 currentPos = _currentPos;
                for (int a = 0; a < borderData[currentArea].Count; ++a)
                {
                    PathData newPathData = borderData[currentArea][a].GetMinPathData(currentPos);
                    newPathData.pathArea.Add(currentArea);
                    TreeNode<PathData> newPathNode = prevNode.AddItem(newPathData);
                    if (newPathNode.Data.desArea != endArea)
                    {
                        currentData.Add(newPathNode);
                    }
                    else
                    {
                        endPathData.Add(newPathNode.AddItem(new PathData(_desPos, (newPathNode.Data.destination - (Vector2)_desPos).sqrMagnitude, endArea)));
                    }
                }
                isFirst = false;
            }
            else
            {
                for (int n = 0; n < prevData.Count; ++n)
                {
                    TreeNode<PathData> prevNode = prevData[n];
                    Area currentArea = prevNode.Data.desArea;
                    Vector3 currentPos = prevNode.Data.destination;
                    HashSet<Area> pathArea = prevNode.Data.pathArea;
                    for (int a = 0; a < borderData[currentArea].Count; ++a)
                    {
                        Border border = borderData[currentArea][a];
                        if (!pathArea.Contains(border.targetArea) && prevNode.root.Data.desArea != border.targetArea)
                        {
                            PathData newPathData = borderData[currentArea][a].GetMinPathData(currentPos);
                            newPathData.pathArea = new HashSet<Area>(pathArea);
                            newPathData.pathArea.Add(border.targetArea);
                            TreeNode<PathData> newPathNode = prevNode.AddItem(newPathData);
                            if (newPathNode.Data.desArea != endArea)
                            {
                                currentData.Add(newPathNode);
                            }
                            else
                            {
                                endPathData.Add(newPathNode.AddItem(new PathData(_desPos, (newPathNode.Data.destination - (Vector2)_desPos).sqrMagnitude, endArea)));
                            }
                        }
                    }
                }
            }

            if (currentData.Count <= 0) break;
            else
            {
                prevData = currentData;
            }
        }

        float minCost = float.PositiveInfinity;
        List<Vector3> minPathData = new List<Vector3>();

        foreach(TreeNode<PathData> node in endPathData)
        {
            float cost = 0;
            List<Vector3> pathData = new List<Vector3>();
            TreeNode<PathData> currentNode = node;
            while(currentNode != null)
            {
                cost += currentNode.Data.distance;
                pathData.Add(currentNode.Data.destination);
                currentNode = currentNode.root;
            }
            pathData.Reverse();
            if(cost < minCost)
            {
                minCost = cost;
                minPathData = pathData;
            }
        }

        return minPathData;
    }

}

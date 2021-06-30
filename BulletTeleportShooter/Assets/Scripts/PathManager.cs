using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    private class Node
    {
        public Vector2 pos;
        public bool isWalkable;
        public int gCost;
        public int hCost;
        public Node parent;

        public override string ToString()
        {
            return $"{pos} : {isWalkable}";
        }
    }


    public static PathManager Instance { get; private set; } = null;
    [SerializeField] private Grid grid;
    [Header("- Border")]
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightUp;
    private Dictionary<Vector2, Node> mapData;


    private void Awake()
    {
        Instance = this;
        mapData = MakeMapData();
    }

    Dictionary<Vector2, Node> MakeMapData()
    {
        Vector2Int leftDownPos = ((Vector2Int)grid.WorldToCell(leftDown.position));
        Vector2Int rightUpPos = ((Vector2Int)grid.WorldToCell(rightUp.position));
        Vector2 cellSize = new Vector2(grid.cellSize.x, grid.cellSize.y);
        Vector2 mapSize = new Vector2((rightUpPos.x - leftDownPos.x) / cellSize.x, (rightUpPos.y - leftDownPos.y) / cellSize.y);
        
        Dictionary<Vector2, Node> newMapData = new Dictionary<Vector2, Node>((int)(mapSize.x * mapSize.y));
        for(int x = 0; x < mapSize.x; ++x)
        {
            for(int y = 0; y < mapSize.y; ++y)
            {
                Vector2 cellPos = new Vector2(leftDownPos.x + cellSize.x * x, leftDownPos.y + cellSize.y * y);
                RaycastHit2D[] hit2D = Physics2D.RaycastAll(new Vector2(cellPos.x + cellSize.x * .5f, cellPos.y + cellSize.y * .5f), Vector2.zero);
                Node newNode = new Node();
                newNode.pos = cellPos;
                newNode.isWalkable = true;
                for (int i = 0; i < hit2D.Length; ++i)
                {
                    if(hit2D[i].collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
                    {
                        newNode.isWalkable = false;
                        break;
                    }
                }
                newMapData.Add(cellPos, newNode);
            }
        }
        return newMapData;
    }
    /// <summary>
    /// 길찾기를 한다. (도착불가 상황일때는 버그 발생 예정)
    /// </summary>
    /// <param name="_currentPos">현재 위치</param>
    /// <param name="_desPos">갈 위치</param>
    public List<Vector3> FindPath(Vector3 _currentPos, Vector3 _desPos)
    {
        Vector3Int currentPos = grid.WorldToCell(_currentPos);
        Vector3Int desPos = grid.WorldToCell(_desPos);
        Node startNode = mapData[new Vector2(currentPos.x, currentPos.y)];
        Node endNode = mapData[new Vector2(desPos.x, desPos.y)];
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 0; i < openSet.Count; ++i)
            {
                if(openSet[i].gCost < currentNode.gCost || openSet[i].gCost == currentNode.gCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode) break;

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if(neighbor.isWalkable == false || closedSet.Contains(neighbor)) continue;

                int newCost = currentNode.gCost + GetCost(currentNode, neighbor);
                if(openSet.Contains(neighbor) == false || newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = GetCost(neighbor, endNode);
                    neighbor.parent = currentNode;
                    if(openSet.Contains(neighbor) == false)
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        List<Vector3> wayPoints = new List<Vector3>();
        Node current = endNode;

        while (current != startNode)
        {
            wayPoints.Add(current.pos);
            current = current.parent;
        }

        List<Vector3> path = new List<Vector3>(wayPoints.Count);
        Vector2 oldDir = Vector2.zero;
        for(int i = 1; i < wayPoints.Count; ++i)
        {
            Vector2 newDir = new Vector2(wayPoints[i - 1].x - wayPoints[i].x, wayPoints[i - 1].y - wayPoints[i].y);
            if(newDir != oldDir)
            {
                path.Add(new Vector3(wayPoints[i - 1].x + grid.cellSize.x * .5f, wayPoints[i - 1].y + grid.cellSize.y * .5f));
            }
            oldDir = newDir;
        }
        path.Add(new Vector3(startNode.pos.x + grid.cellSize.x * .5f, startNode.pos.y + grid.cellSize.y * .5f));
        path.Reverse();

        for(int i = 0; i < path.Count; ++i)
        {
            Debug.Log(path[i]);
        }

        foreach (KeyValuePair<Vector2, Node> kv in mapData)
        {
            // 일단 임시방편으로 이렇게 해두긴 했는데, 이렇게 하면은 한 번 계산할때마다 코스트가 어마어마할 것이다.
            // 반드시 개선방안을 찾도록 하자!!!!!!
            kv.Value.parent = null;
        }

        return path;

        int GetCost(Node _current, Node _des)
        {
            int x = (int)Mathf.Abs(_current.pos.x - _des.pos.y);
            int y = (int)Mathf.Abs(_current.pos.y - _des.pos.y);

            if(x > y)
            {
                return 14 * y + 10 * (x - y);
            }
            return 14 * x + 10 * (y - x);
        }

        List<Node> GetNeighbors(Node _node)
        {
            List<Node> neighbors = new List<Node>();
            float[,] posInfo = { { 0, grid.cellSize.y }, { 0, -grid.cellSize.y }, { -grid.cellSize.x, 0 }, { grid.cellSize.x, 0 } };
            bool[] walkableDiagonal = new bool[4];

            for(int i = 0; i < 4; ++i)
            {
                Vector2 checkPos = new Vector2(_node.pos.x + posInfo[i, 0], _node.pos.y + posInfo[i, 1]);
                if(leftDown.position.x <= checkPos.x && checkPos.x < rightUp.position.x && leftDown.position.y <= checkPos.y && checkPos.y < rightUp.position.y)
                {
                    walkableDiagonal[i] = mapData[checkPos].isWalkable;
                    neighbors.Add(mapData[checkPos]);
                }
            }

            for(int i = 0; i < 4; ++i)
            {
                if(walkableDiagonal[i] && walkableDiagonal[3 - i])
                {
                    Vector2 checkPos = new Vector2(
                        _node.pos.x + posInfo[i, 0] + posInfo[3 - i, 0],
                        _node.pos.y + posInfo[i, 1] + posInfo[3 - i, 1]);
                    if (leftDown.position.x <= checkPos.x && checkPos.x < rightUp.position.x && leftDown.position.y <= checkPos.y && checkPos.y < rightUp.position.y)
                    {
                        neighbors.Add(mapData[checkPos]);
                    }
                }
            }

            return neighbors;
        }
    }
}

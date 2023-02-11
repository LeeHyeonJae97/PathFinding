using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private AStarPathFinder _astar;
    [SerializeField] private JPSPathFinder _jps;

    private void OnValidate()
    {
        //_astar.OnValidate();
        _jps.OnValidate();
    }

    private void Awake()
    {
        //_astar.Awake();
        _jps?.Awake();
    }

    private void Update()
    {
        //_astar.Update();
        _jps.Update();
    }

    private void OnDrawGizmos()
    {
        //_astar.OnDrawGizmos();
        _jps?.OnDrawGizmos();
    }

    [System.Serializable]
    private class AStarPathFinder
    {
        private List<AStarNode> path = new List<AStarNode>();

        public void Awake()
        {
            var nodes = new AStarNode[5, 5];

            for (int x = 0; x < nodes.GetLength(0); x++)
            {
                for (int y = 0; y < nodes.GetLength(1); y++)
                {
                    nodes[x, y] = new AStarNode(new Vector2Int(x, y));
                }
            }

            nodes[1, 0].IsWalkable = false;
            nodes[1, 1].IsWalkable = false;
            nodes[1, 2].IsWalkable = false;
            nodes[1, 3].IsWalkable = false;
            nodes[3, 4].IsWalkable = false;
            nodes[3, 3].IsWalkable = false;
            nodes[3, 2].IsWalkable = false;
            nodes[3, 1].IsWalkable = false;

            Debug.Log(AStar.Find(nodes, Vector2Int.zero, new Vector2Int(4, 4), path));
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                foreach (var node in path)
                {
                    node.DrawGizmos(Color.red);
                }
            }
        }
    }

    [System.Serializable]
    private class JPSPathFinder
    {
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Vector2Int _start;
        [SerializeField] private Vector2Int _end;
        private JPSNode[,] _nodes;
        private List<JPSNode> _path = new List<JPSNode>();

        public void OnValidate()
        {
            _nodes = new JPSNode[_size.x, _size.y];

            for (int x = 0; x < _nodes.GetLength(0); x++)
            {
                for (int y = 0; y < _nodes.GetLength(1); y++)
                {
                    _nodes[x, y] = new JPSNode(new Vector2Int(x, y));
                }
            }
        }

        public void Awake()
        {

        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouse = Input.mousePosition;
                mouse.z -= Camera.main.transform.position.z;

                var position = Camera.main.ScreenToWorldPoint(mouse);

                for (int i = 0; i < _nodes.GetLength(0); i++)
                {
                    for (int j = 0; j < _nodes.GetLength(1); j++)
                    {
                        var bounds = new Bounds((Vector3Int)_nodes[i, j].Position, Vector2.one * 0.8f);

                        if (bounds.Contains(position))
                        {
                            _nodes[i, j].IsWalkable = !_nodes[i, j].IsWalkable;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(JPS.Find(_nodes, _start, _end, _path));
            }
        }

        public void OnDrawGizmos()
        {
            if (_nodes != null && _path != null)
            {
                foreach (var node in _nodes)
                {
                    Gizmos.DrawWireRect(node.Position, Vector2.one * 0.8f, node.Position == _start ? Color.green : node.Position == _end ? Color.blue : node.IsWalkable ? _path.Contains(node) ? Color.red : Color.white : Color.black);
                }
            }
        }
    }
}

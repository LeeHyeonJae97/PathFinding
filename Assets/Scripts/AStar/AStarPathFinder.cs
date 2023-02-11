using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AStarPathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _start;
    [SerializeField] private Vector2Int _end;
    [SerializeField] private Transform _obstacles;
    [SerializeField] private TextMeshProUGUI _text;
    private AStarNode[,] _nodes;
    private List<AStarNode> _path = new List<AStarNode>();

    private void Awake()
    {
        _nodes = new AStarNode[_size.x, _size.y];

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                _nodes[x, y] = new AStarNode(new Vector2Int(x, y));
            }
        }
    }

    private void Update()
    {
        GetObstacles();
        Find();

        void GetObstacles()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _nodes[x, y].IsWalkable = true;
                }
            }

            foreach (Transform obsctacle in _obstacles)
            {
                var bounds = obsctacle.GetComponent<Obstacle>().Bounds;

                var min = bounds.min;
                var max = bounds.max;

                for (int x = Mathf.CeilToInt(min.x); x <= Mathf.FloorToInt(max.x); x++)
                {
                    for (int y = Mathf.CeilToInt(min.y); y <= Mathf.FloorToInt(max.y); y++)
                    {
                        if (0 <= x && x < _size.x && 0 <= y && y < _size.y)
                        {
                            _nodes[x, y].IsWalkable = false;
                        }
                    }
                }
            }
        }

        void Find()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!(0 <= _start.x && _start.x < _size.x && 0 <= _start.y && _start.y < _size.y)) return;

                if (!(0 <= _end.x && _end.x < _size.x && 0 <= _end.y && _end.y < _size.y)) return;

                var stopwatch = new System.Diagnostics.Stopwatch();

                stopwatch.Start();
                AStar.Find(_nodes, _start, _end, _path);
                stopwatch.Stop();

                _text.text = $"{stopwatch.ElapsedMilliseconds}";
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (_nodes != null && _path != null)
        {
            foreach (var node in _nodes)
            {
                Gizmos.DrawWireRect(node.Position, Vector2.one * 0.8f, node.Position == _start ? Color.green : node.Position == _end ? Color.blue : node.IsWalkable ? _path.Contains(node) ? Color.red : Color.white : Color.black);
            }
        }
    }
}

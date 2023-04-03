using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JPSPathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _start;
    [SerializeField] private Transform _obstacles;
    [SerializeField] private TextMeshProUGUI _text;
    private PathNode[,] _nodes;
    private List<PathNode> _path = new List<PathNode>();

    private void Start()
    {
        Initialize();
        GetObstacles();
    }

    private void Update()
    {
        Initialize();
        Find();
    }

    private void Initialize()
    {
        if (_nodes == null)
        {
            _nodes = new PathNode[_size.x, _size.y];

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _nodes[x, y] = new PathNode(new Vector2Int(x, y));
                }
            }
        }
        else
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _nodes[x, y].Reset();
                }
            }
        }
    }

    private void GetObstacles()
    {
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

    private void Find()
    {
        if (!(0 <= _start.x && _start.x < _size.x && 0 <= _start.y && _start.y < _size.y)) return;

        var mousePos = Input.mousePosition;
        mousePos.z -= Camera.main.transform.position.z;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        var end = new Vector2Int((int)mousePos.x, (int)mousePos.y);
        end.x = Mathf.Clamp(end.x, 0, _size.x - 1);
        end.y = Mathf.Clamp(end.y, 0, _size.y - 1);

        var stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Start();
        bool success = JPS.Find(_nodes, _start, end, _path);
        stopwatch.Stop();

        _text.text = $"{stopwatch.ElapsedMilliseconds}";
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireRect(_size / 2, _size);

        if (!Application.isPlaying) return;

        if (_nodes != null && _path != null)
        {
            foreach (var node in _nodes)
            {
                //UnityEditor.Handles.Label((Vector2)node.Position, $"{node.Searched}");
                Gizmos.DrawWireRect(node.Position, Vector2.one * 0.8f, node.Position == _start ? Color.green : node.IsWalkable ? _path.Contains(node) ? Color.red : Color.white : Color.black);
            }
        }
    }
}

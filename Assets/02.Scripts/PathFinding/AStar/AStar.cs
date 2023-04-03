using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static readonly Vector2Int[] _neighbors = { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1) };
    private static readonly MinHeap<PathNode> _open = new MinHeap<PathNode>(10000);

    public static bool Find(PathNode[,] nodes, Vector2Int start, Vector2Int end, List<PathNode> path)
    {
        path.Clear();
        _open.Clear();

        var initial = nodes[start.x, start.y];
        initial.State = 1;
        _open.Add(initial);

        while (_open.Count > 0)
        {
            //Debug.Log("A");

            var current = _open.Pop();
            current.State = 2;

            if (current == nodes[end.x, end.y])
            {
                while (current != null && (current.Position == start || current.Parent != null))
                {
                    path.Add(current);
                    current = current.Parent;
                }
                path.Reverse();

                return true;
            }

            for (int i = 0; i < _neighbors.Length; i++)
            {
                var position = current.Position + _neighbors[i];

                if (0 <= position.x && position.x < nodes.GetLength(0) && 0 <= position.y && position.y < nodes.GetLength(1))
                {
                    var neighbor = nodes[position.x, position.y];

                    if (neighbor.IsWalkable && neighbor.State != 2)
                    {
                        var diagonal = i % 2 == 1;
                        var dst = diagonal ? 14 : 10;

                        var g = current.G + dst;

                        if (neighbor.State == 0 || g < neighbor.G)
                        {
                            neighbor.G = g;
                            neighbor.H = Mathf.Abs(end.x - position.x) + Mathf.Abs(end.y - position.y);
                            neighbor.Parent = current;

                            if (neighbor.State == 0)
                            {
                                neighbor.State = 1;
                                _open.Add(neighbor);
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}

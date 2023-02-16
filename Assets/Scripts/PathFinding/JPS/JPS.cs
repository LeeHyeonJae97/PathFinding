using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS
{
    private static readonly Vector2Int[] _neighbors = { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1) };
    private static readonly MinHeap<PathNode> _open = new MinHeap<PathNode>(10000);

    public static bool Find(PathNode[,] nodes, Vector2Int start, Vector2Int end, List<PathNode> path)
    {
        path.Clear();

        _open.Clear();

        var startNode = nodes[start.x, start.y];
        startNode.State = 1;
        _open.Add(startNode);

        while (_open.Count > 0)
        {
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

            for (int dir = 0; dir < _neighbors.Length; dir++)
            {
                if (current.Parent == null ||
                    current.Direction == dir ||
                    (current.Direction + 7) % 8 == dir || (current.Direction + 1) % 8 == dir ||
                    ((current.Direction % 2 != 0) && ((current.Direction + 6) % 8 == dir || (current.Direction + 2) % 8 == dir)))
                {

                    var position = current.Position + _neighbors[dir];

                    if (Jump(position, dir, out var forcedNeighbor))
                    {
                        var x = Mathf.Abs(current.Position.x - forcedNeighbor.Position.x);
                        var y = Mathf.Abs(current.Position.y - forcedNeighbor.Position.y);

                        var diagonal = dir % 2 == 1;
                        var dst = diagonal ? x * 14 : (x + y) * 10;

                        var g = current.G + dst;

                        if (forcedNeighbor.State == 0 || g < forcedNeighbor.G)
                        {
                            forcedNeighbor.G = g;
                            forcedNeighbor.H = (Mathf.Abs(end.x - forcedNeighbor.Position.x) + Mathf.Abs(end.y - forcedNeighbor.Position.y)) * 10;
                            forcedNeighbor.Parent = current;
                            forcedNeighbor.Direction = dir;

                            if (forcedNeighbor.State == 0)
                            {
                                forcedNeighbor.State = 1;
                                _open.Add(forcedNeighbor);
                            }
                        }
                    }
                }
            }
        }

        return false;

        bool Jump(Vector2Int position, int direction, out PathNode forcedNeighbor)
        {
            if (!Validate(position))
            {
                forcedNeighbor = null;
                return false;
            }

            var current = nodes[position.x, position.y];

            current.Searched++;

            if (!current.IsWalkable || current.State == 2)
            {
                forcedNeighbor = null;
                return false;
            }

            if (current == nodes[end.x, end.y])
            {
                forcedNeighbor = current;
                return true;
            }

            bool cardinal = direction % 2 == 0;

            for (int j = 0; j < 2; j++)
            {
                var walkablePosition = current.Position + _neighbors[(direction + (cardinal ? j == 0 ? 7 : 1 : j == 0 ? 6 : 2)) % _neighbors.Length];
                var notWalkablePosition = current.Position + _neighbors[(direction + (cardinal ? j == 0 ? 6 : 2 : j == 0 ? 5 : 3)) % _neighbors.Length];

                if (Validate(walkablePosition) && Validate(notWalkablePosition))
                {
                    var walkableNode = nodes[walkablePosition.x, walkablePosition.y];
                    var notWalkableNode = nodes[notWalkablePosition.x, notWalkablePosition.y];

                    if (walkableNode.IsWalkable && !notWalkableNode.IsWalkable)
                    {
                        forcedNeighbor = current;
                        return true;
                    }
                }
            }

            if (!cardinal)
            {
                if (Jump(position + _neighbors[direction - 1], direction - 1, out forcedNeighbor) || Jump(position + _neighbors[(direction + 1) % 8], (direction + 1) % 8, out forcedNeighbor))
                {
                    forcedNeighbor = current;
                    return true;
                }
            }

            return Jump(position + _neighbors[direction], direction, out forcedNeighbor);
        }

        bool Validate(Vector2Int position)
        {
            return 0 <= position.x && position.x < nodes.GetLength(0) && 0 <= position.y && position.y < nodes.GetLength(1);
        }
    }
}
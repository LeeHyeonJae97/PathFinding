using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPS
{
    private static readonly Vector2Int[] _neighbors = { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1) };

    public static bool Find(JPSNode[,] nodes, Vector2Int start, Vector2Int end, List<JPSNode> path)
    {
        path.Clear();

        SortedSet<JPSNode> open = new SortedSet<JPSNode>();
        HashSet<JPSNode> closed = new HashSet<JPSNode>();

        open.Add(nodes[start.x, start.y]);

        int count = 0;

        while (open.Count > 0 && count++ < 100)
        {
            var current = open.Min;

            closed.Add(current);
            open.Remove(current);

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

                if (Find(position, i, out var forcedNeighbor))
                {
                    var x = Mathf.Abs(current.Position.x - forcedNeighbor.Position.x);
                    var y = Mathf.Abs(current.Position.y - forcedNeighbor.Position.y);

                    var diagonal = i % 2 == 1;
                    var dst = diagonal ? x * 14 : (x + y) * 10;

                    var g = current.G + dst;

                    if (!open.Contains(forcedNeighbor) || g < forcedNeighbor.G)
                    {
                        forcedNeighbor.G = g;
                        forcedNeighbor.H = Mathf.Abs(end.x - position.x) + Mathf.Abs(end.y - position.y);
                        forcedNeighbor.Parent = current;

                        if (!open.Contains(forcedNeighbor))
                        {
                            open.Add(forcedNeighbor);
                        }
                    }
                }
            }
        }

        return false;

        bool Find(Vector2Int position, int index, out JPSNode forcedNeighbor)
        {
            if (!Movable(position))
            {
                forcedNeighbor = null;
                return false;
            }

            var current = nodes[position.x, position.y];

            if (!current.IsWalkable || closed.Contains(current))
            {
                forcedNeighbor = null;
                return false;
            }

            if (current == nodes[end.x, end.y])
            {
                forcedNeighbor = current;
                return true;
            }

            bool straight = index % 2 == 0;

            for (int j = 0; j < 2; j++)
            {
                var walkablePosition = current.Position + ((Vector2)(Quaternion.Euler(0, 0, (straight ? 45 : 90) * (j == 0 ? 1 : -1)) * (Vector2)_neighbors[index]).Round()).ToInt();
                var notWalkablePosition = current.Position + ((Vector2)(Quaternion.Euler(0, 0, (straight ? 90 : 135) * (j == 0 ? 1 : -1)) * (Vector2)_neighbors[index]).Round()).ToInt();

                if (Movable(walkablePosition) && Movable(notWalkablePosition))
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

            if (!straight)
            {
                if (Find(position, index - 1, out forcedNeighbor) || Find(position, (index + 1) % 8, out forcedNeighbor))
                {
                    forcedNeighbor = current;
                    return true;
                }
            }

            return Find(position + _neighbors[index], index, out forcedNeighbor);
        }

        bool Movable(Vector2Int position)
        {
            return 0 <= position.x && position.x < nodes.GetLength(0) && 0 <= position.y && position.y < nodes.GetLength(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static readonly Vector2Int[] _neighbors = { new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, -1), new Vector2Int(-1, -1) };

    public static bool Find(AStarNode[,] nodes, Vector2Int start, Vector2Int end, List<AStarNode> path)
    {
        path.Clear();

        SortedSet<AStarNode> open = new SortedSet<AStarNode>();
        HashSet<AStarNode> closed = new HashSet<AStarNode>();

        open.Add(nodes[start.x, start.y]);

        while (open.Count > 0)
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

                if (0 <= position.x && position.x < nodes.GetLength(0) && 0 <= position.y && position.y < nodes.GetLength(1))
                {
                    var neighbor = nodes[position.x, position.y];

                    if (neighbor.IsWalkable && !closed.Contains(neighbor))
                    {
                        var diagonal = i % 2 == 1;
                        var dst = diagonal ? 14 : 10;

                        var g = current.G + dst;

                        if (!open.Contains(neighbor) || g < neighbor.G)
                        {
                            neighbor.G = g;
                            neighbor.H = Mathf.Abs(end.x - position.x) + Mathf.Abs(end.y - position.y);
                            neighbor.Parent = current;

                            if (!open.Contains(neighbor))
                            {
                                open.Add(neighbor);
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}

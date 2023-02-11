using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IComparable<AStarNode>
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }
    public AStarNode Parent { get; set; }
    public Vector2Int Position { get; private set; }
    public bool IsWalkable { get; set; }

    public AStarNode(Vector2Int position)
    {
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        IsWalkable = true;
    }

    public AStarNode(Vector2Int position, bool isWalkable)
    {
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        IsWalkable = isWalkable;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.DrawWireCube(new Vector3(Position.x, Position.y, 0), Vector2.one, color);
    }

    public int CompareTo(AStarNode other)
    {
        if (Position == other.Position) return 0;

        var comp = F.CompareTo(other.F);

        if (comp == 0)
        {
            comp = H.CompareTo(other.H);

            if (comp == 0)
            {
                comp = GetHashCode().CompareTo(other.GetHashCode());
            }
        }

        return comp;
    }
}

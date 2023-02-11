using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPSNode : IComparable<JPSNode>
{
    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }
    public JPSNode Parent { get; set; }
    public Vector2Int Position { get; private set; }
    public bool IsWalkable { get; set; }

    public JPSNode(Vector2Int position)
    {
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        IsWalkable = true;
    }

    public JPSNode(Vector2Int position, bool isWalkable)
    {
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        IsWalkable = isWalkable;
    }

    public int CompareTo(JPSNode other)
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

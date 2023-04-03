using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IComparable<PathNode>
{
    // 0 : Not Discovered
    // 1 : Opened
    // 2 : Closed
    public byte State { get; set; }
    public int Searched { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get { return G + H; } }
    public PathNode Parent { get; set; }
    public Vector2Int Position { get; private set; }
    public int Direction { get; set; }
    public bool IsWalkable { get; set; }

    public PathNode(Vector2Int position)
    {
        State = 0;
        Searched = 0;
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        Direction = 0;
        IsWalkable = true;
    }

    public PathNode(Vector2Int position, bool isWalkable)
    {
        State = 0;
        Searched = 0;
        G = 0;
        H = 0;
        Parent = null;
        Position = position;
        Direction = 0;
        IsWalkable = isWalkable;
    }

    public void Reset()
    {
        State = 0;
        Searched = 0;
        G = 0;
        H = 0;
        Parent = null;
        Direction = 0;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.DrawWireCube(new Vector3(Position.x, Position.y, 0), Vector2.one, color);
    }

    public int CompareTo(PathNode other)
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

﻿using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct AxialCoordinate
{
    public static AxialCoordinate Zero      = new AxialCoordinate(0, 0);
    public static AxialCoordinate UpRight   = new AxialCoordinate(0, 1);
    public static AxialCoordinate Right     = new AxialCoordinate(1, 0);
    public static AxialCoordinate DownRight = new AxialCoordinate(1, -1);
    public static AxialCoordinate DownLeft  = new AxialCoordinate(0, -1);
    public static AxialCoordinate Left      = new AxialCoordinate(-1, 0);
    public static AxialCoordinate UpLeft    = new AxialCoordinate(-1, 1);

    public static AxialCoordinate[] Directions = new AxialCoordinate[]
    {
            UpRight,
            Right,
            DownRight,
            DownLeft,
            Left,
            UpLeft
    };

    public int X { get { return x; } }
    public int Z { get { return z; } }

    /// <summary>
    /// Convert axial coordinate x to a column index in a grid layout.
    /// </summary>
    public int GridColumn { get { return x + z / 2; } }

    [SerializeField]
    private int x;
    [SerializeField]
    private int z;

    public AxialCoordinate(int x, int y)
    {
        this.x = x;
        this.z = y;
    }

    public bool IsAdjacent(AxialCoordinate other)
    {
        return Directions.Select(d => other + d).Contains(this);
    }

    public HexDirection GetDirection(AxialCoordinate adjacent)
    {
        AxialCoordinate directionCoordinate = adjacent - this;
        int directionIndex = Directions.TakeWhile(d => d != directionCoordinate).Sum(d => 1);

        return (HexDirection)directionIndex;
    }

    /// <summary>
    /// Calculate an axial coordinate from a column and row index in a grid layout. Column and Row
    /// indices are assumed to start at the bottom left of the grid layout
    /// </summary>
    public static AxialCoordinate FromGridIndices(int column, int row)
    {
        return new AxialCoordinate(column - row / 2, row);
    }

    public static AxialCoordinate operator +(AxialCoordinate a, AxialCoordinate b)
    {
        return new AxialCoordinate(a.X + b.X, a.Z + b.Z);
    }

    public static AxialCoordinate operator -(AxialCoordinate a, AxialCoordinate b)
    {
        return new AxialCoordinate(a.X - b.X, a.Z - b.Z);
    }

    public static AxialCoordinate operator *(AxialCoordinate a, int scale)
    {
        return new AxialCoordinate(a.X * scale, a.Z * scale);
    }

    public static AxialCoordinate operator /(AxialCoordinate a, int scale)
    {
        return new AxialCoordinate(a.X / scale, a.Z / scale);
    }

    public static bool operator ==(AxialCoordinate a, AxialCoordinate b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(AxialCoordinate a, AxialCoordinate b)
    {
        return !(a == b);
    }

    public override bool Equals(object o)
    {
        if (o is AxialCoordinate)
            return this == (AxialCoordinate)o;

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return X.ToString() + ", " + Z.ToString();
    }
}
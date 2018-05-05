using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public struct HexRotation
{
    public static HexRotation Identity = new HexRotation(0);

    public int ClockwiseOrdinal
    {
        get { return clockwiseOrdinal; }
        set { clockwiseOrdinal = Maths.Wrap0(value, 6); }
    }
    public int Degrees { get { return 30 + (ClockwiseOrdinal * 60); } }

    [SerializeField]
    private int clockwiseOrdinal;

    private HexRotation(int clockwiseOrdinal)
    {
        int wrappedOrdinal = Maths.Wrap0(clockwiseOrdinal, 6);
        this.clockwiseOrdinal = wrappedOrdinal;
    }

    public static HexRotation FromOrdinal(int clockwiseOrdinal)
    {
        return new HexRotation(clockwiseOrdinal);
    }

    public static HexRotation FromDirection(HexDirection direction)
    {
        return FromOrdinal((int)direction);
    }

    public static HexRotation FromDegrees(int degrees)
    {
        return new HexRotation((degrees - 30) / 60);
    }

    public static HexRotation FromCoordinates(AxialCoordinate axis, AxialCoordinate other)
    {
        return FromDirection(axis.GetDirection(other));
    }

    public HexDirection ToDirection()
    {
        return Maths.Wrap<HexDirection>(ClockwiseOrdinal);
    }

    public Quaternion ToQuaternion()
    {
        return Quaternion.Euler(0, Degrees, 0);
    }

    public AxialCoordinate ToCoordinate()
    {
        return ToDirection().ToCoordinate();
    }

    //public static implicit operator HexDirection(HexRotation rotation)
    //{
    //    return rotation.ToDirection();
    //}

    public static HexRotation operator +(HexRotation a, HexRotation b)
    {
        return FromOrdinal(a.ClockwiseOrdinal + b.ClockwiseOrdinal);
    }

    public static HexRotation operator -(HexRotation a, HexRotation b)
    {
        return FromOrdinal(a.ClockwiseOrdinal - b.ClockwiseOrdinal);
    }

    public static HexRotation operator *(HexRotation rotation, int scale)
    {
        return FromOrdinal(rotation.ClockwiseOrdinal * scale);
    }

    public static HexRotation operator /(HexRotation rotation, int scale)
    {
        return FromOrdinal(rotation.ClockwiseOrdinal / scale);
    }
}

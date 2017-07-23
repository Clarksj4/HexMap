using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum HexDirection
{
    UpRight,
    Right,
    DownRight,
    DownLeft, 
    Left, 
    UpLeft
}

public static class HexDirectionExtension
{
    public static AxialCoordinate ToCoordinate(this HexDirection direction)
    {
        return AxialCoordinate.Directions[(int)direction];
    }

    public static Vector3 ToNormalizedVector(this HexDirection direction)
    {
        return direction.ToRotation() * Vector3.forward;
    }

    public static Quaternion ToRotation(this HexDirection direction)
    {
        // Calculate rotation based off of coordinate position in Directions array
        float rotation = 30 + (int)direction * 60;
        return Quaternion.Euler(0, rotation, 0);
    }

    public static HexDirection Opposite(this HexDirection direction)
    {
        int unwrapped = (int)direction + (AxialCoordinate.Directions.Length / 2);
        int wrapped = Maths.Wrap(unwrapped, 0, AxialCoordinate.Directions.Length);
        return (HexDirection)wrapped;
    }
}

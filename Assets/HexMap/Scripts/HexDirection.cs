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
        return direction.ToQuaternion() * Vector3.forward;
    }

    public static HexRotation ToRotation(this HexDirection direction)
    {
        return HexRotation.FromOrdinal((int)direction);
    }

    public static Quaternion ToQuaternion(this HexDirection direction)
    {
        return HexRotation.FromDirection(direction).ToQuaternion();
    }

    public static HexDirection Opposite(this HexDirection direction)
    {
        int unwrapped = (int)direction + (AxialCoordinate.Directions.Length / 2);
        int wrapped = Maths.Wrap(unwrapped, 0, AxialCoordinate.Directions.Length);
        return (HexDirection)wrapped;
    }
}

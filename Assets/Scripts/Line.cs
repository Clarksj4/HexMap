using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line : Shape
{
    [SerializeField][HideInInspector]
    private string Type = "Line";

    /// <summary>
    /// The offset from the origin
    /// </summary>
    public AxialCoordinate Offset;

    /// <summary>
    /// The direction the line extends in
    /// </summary>
    public HexDirection Direction;

    /// <summary>
    /// How far the line extends. One = only the origin
    /// </summary>
    public int Range;

    /// <summary>
    /// Returns each coordinates in this lines area of effect. The line originates
    /// from its offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> FromOffset()
    {
        return From(AxialCoordinate.Zero);
    }

    /// <summary>
    /// Returns each corrdainte in this line's area of effect. The line originates
    /// from the given origin, offset by its offset coordinate
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin)
    {
        return From(origin + Offset, Direction, Range);
    }

    /// <summary>
    /// Gets each coordinate within the line's range
    /// </summary>
    public static IEnumerable<AxialCoordinate> From(AxialCoordinate origin, HexDirection direction, int range)
    {
        // Scalable numeric direction
        AxialCoordinate directionIncrement = direction.ToCoordinate();

        // Return each coordinate in the line's range
        for (int i = 0; i < range; i++)
            yield return origin + (directionIncrement * i);
    }
}

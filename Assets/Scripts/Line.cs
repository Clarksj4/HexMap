using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Line : Shape
{
    /// <summary>
    /// String representation of the type. Serialized so that type 
    /// can be identified from json
    /// </summary>
    [SerializeField][HideInInspector]
    private string Type = typeof(Line).ToString();

    /// <summary>
    /// How much to offset the shape's area of effect
    /// </summary>
    public AxialCoordinate Offset { get { return offset; } }
    [SerializeField]
    private AxialCoordinate offset;

    /// <summary>
    /// The direction the line extends in
    /// </summary>
    public HexDirection Direction;

    /// <summary>
    /// How far the line extends. One = only the origin
    /// </summary>
    public int Range;

    /// <summary>
    /// Returns each coordinates in this shape's area of effect. The shape originates
    /// from the given origin translated by the shape's offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin)
    {
        return From(origin + Offset, Direction, Range);
    }

    /// <summary>
    /// Returns each coordinates in this lines area of effect. The line originates
    /// from its offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> FromOffset()
    {
        return From(AxialCoordinate.Zero);
    }

    /// <summary>
    /// The collection of coordinates with the defined line's area.
    /// </summary>
    public static IEnumerable<AxialCoordinate> From(AxialCoordinate origin, HexDirection direction, int range)
    {
        // Scalable numeric direction
        AxialCoordinate directionIncrement = direction.ToCoordinate();

        // Return each coordinate in the line's range
        for (int i = 0; i <= range; i++)
            yield return origin + (directionIncrement * i);
    }
}

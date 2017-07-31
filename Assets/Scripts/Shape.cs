using System.Collections.Generic;

public interface Shape
{
    /// <summary>
    /// How much to offset the shape's area of effect
    /// </summary>
    AxialCoordinate Offset { get; }

    /// <summary>
    /// Returns each coordinates in this shape's area of effect. The shape originates
    /// from the given origin translated by the shape's offset coordinate
    /// </summary>
    IEnumerable<AxialCoordinate> From(AxialCoordinate origin);

    /// <summary>
    /// Returns each coordinates in this shape's area of effect. The shape originates
    /// from the shape's offset coordinate
    /// </summary>
    IEnumerable<AxialCoordinate> FromOffset();
}

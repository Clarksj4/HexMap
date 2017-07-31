using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

[Serializable]
public class Ring : Shape
{
    /// <summary>
    /// String representation of the type. Serialized so that type 
    /// can be identified from json
    /// </summary>
    [SerializeField][HideInInspector]
    private string Type = "Ring";

    /// <summary>
    /// The offset from the origin
    /// </summary>
    public AxialCoordinate Offset { get { return offset; } }
    [SerializeField]
    private AxialCoordinate offset;

    /// <summary>
    /// The minimum distance away that will be affected by the quake
    /// </summary>
    public int Minimum;
    
    /// <summary>
    /// The maximum distance away that will be affected by the quake
    /// </summary>
    public int Maximum;

    /// <summary>
    /// Returns each coordinates in this lines area of effect. The line originates
    /// from its offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> FromOffset()
    {
        return From(AxialCoordinate.Zero);
    }

    /// <summary>
    /// Returns each coordinates in this shape's area of effect. The shape originates
    /// from the given origin translated by the shape's offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin)
    {
        return From(origin + Offset, Minimum, Maximum);
    }

    /// <summary>
    /// The collection of coordinates with the defined ring's area.
    /// </summary>
    public static IEnumerable<AxialCoordinate> From(AxialCoordinate origin, int minimum, int maximum)
    {
        return Pathfind.Enumerate(origin, maximum)
               .Where(step => step.CostTo >= minimum)
               .Select(step => (AxialCoordinate)step.Node);
    }
}

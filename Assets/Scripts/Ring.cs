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
    [HideInInspector]
    public string Type = typeof(Ring).ToString();

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
        return From(AxialCoordinate.Zero, HexRotation.Identity);
    }

    /// <summary>
    /// Returns each coordinates in this shape's area of effect. The shape originates
    /// from the given origin translated by the shape's offset coordinate
    /// </summary>
    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin, HexRotation orientation)
    {
        return From(origin + Offset, Minimum, Maximum);
    }

    /// <summary>
    /// The collection of coordinates with the defined ring's area.
    /// </summary>
    public static IEnumerable<AxialCoordinate> From(AxialCoordinate origin, int minimum, int maximum)
    {
        // TODO: Use a QuakeTraverser that says moving to a cell NOT on the map is not a traversable move
        // make cost to move to each cell 1
        return Pathfind.Enumerate(origin, maximum)
               .Where(step => step.CostTo >= minimum)
               .Select(step => (AxialCoordinate)step.Node);
    }
}

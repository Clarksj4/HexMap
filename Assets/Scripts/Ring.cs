using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

[Serializable]
public class Ring : Shape
{
    [SerializeField][HideInInspector]
    private string Type = "Ring";
    public AxialCoordinate Offset;
    public int Minimum;
    public int Maximum;

    public IEnumerable<AxialCoordinate> FromOffset()
    {
        return From(AxialCoordinate.Zero);
    }

    public IEnumerable<AxialCoordinate> From(AxialCoordinate origin)
    {
        return From(origin + Offset, Minimum, Maximum);
    }

    public static IEnumerable<AxialCoordinate> From(AxialCoordinate origin, int minimum, int maximum)
    {
        return Pathfind.Enumerate(origin, maximum)
               .Where(step => step.CostTo >= minimum)
               .Select(step => (AxialCoordinate)step.Node);
    }
}

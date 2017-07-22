using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public AxialCoordinate Coordinate
    {
        get { return coordinate; }
        set
        {
            coordinate = value;
            cell = map[coordinate];

            transform.position = cell.Position;
        }
    }

    public AxialCoordinate[] Directions { get { return directions; } }

    public Vector3 Position { get { return transform.position; } }

    public GameObject Section;

    [SerializeField]
    private AxialCoordinate coordinate;
    [SerializeField]
    private AxialCoordinate[] directions;
    [SerializeField][HideInInspector]
    private HexCell cell;
    [SerializeField][HideInInspector]
    private HexMap map;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();
    }

    public GameObject AddSection(AxialCoordinate direction)
    {
        if (directions.Contains(direction))
            throw new ArgumentException("Pipe already extends in the given direction");

        GameObject section = Instantiate(Section);

        // Size
        Vector3 scale = new Vector3(section.transform.localScale.x, section.transform.localScale.y, cell.HexMesh.InnerRadius);
        section.transform.localScale = scale;

        // Orientation
        section.transform.LookAt(direction);

        // Position
        section.transform.position = cell.Position + (direction.Vector * (cell.HexMesh.InnerRadius / 2));

        return section;
    }

    public bool IsValidExtension(HexCell origin, AxialCoordinate direction)
    {
        HexCell end = map[origin.Coordinate + direction];

        return origin != null &&
               end != null &&
               origin != end &&
               origin.IsAdjacent(end) &&
               !directions.Contains(direction);
    }
}

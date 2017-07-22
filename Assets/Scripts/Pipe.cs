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
            cell.Add(this);

            transform.position = cell.Position;
        }
    }

    public Vector3 Position { get { return transform.position; } }

    public PipeSection SectionPrefab;

    [SerializeField]
    private AxialCoordinate coordinate;
    [SerializeField][HideInInspector]
    private PipeSection[] sections = new PipeSection[AxialCoordinate.Directions.Length];
    [SerializeField][HideInInspector]
    private HexCell cell;
    [SerializeField][HideInInspector]
    private HexMap map;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();
    }

    public GameObject AddSection(HexDirection direction)
    {
        if (sections[(int)direction] == null)
        {
            // Create section and position / orientate on cell
            PipeSection section = Instantiate(SectionPrefab, transform);
            section.Set(cell, direction);
            sections[(int)direction] = section;

            return section.gameObject;
        }

        return null;
    }

    public bool IsValidExtension(HexCell origin, HexDirection direction)
    {
        HexCell end = map[origin.Coordinate + direction.ToCoordinate()];

        return origin != null &&
               end != null &&
               origin != end &&
               origin.IsAdjacent(end) &&
               !HasSectionInDirection(direction);
    }

    public bool HasSectionInDirection(HexDirection direction)
    {
        return sections[(int)direction] != null;
    }
}

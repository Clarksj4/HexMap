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

    public Vector3 Position { get { return transform.position; } }

    public GameObject SectionPrefab;

    [SerializeField]
    private AxialCoordinate coordinate;
    [SerializeField][HideInInspector]
    private GameObject[] sections;
    [SerializeField][HideInInspector]
    private HexCell cell;
    [SerializeField][HideInInspector]
    private HexMap map;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();

        if (sections == null)
            sections = new GameObject[AxialCoordinate.Directions.Length];
    }

    public GameObject AddSection(HexDirection direction)
    {
        if (sections[(int)direction] == null)
        {
            GameObject section = Instantiate(SectionPrefab);

            // Size
            Vector3 scale = new Vector3(section.transform.localScale.x, section.transform.localScale.y, cell.HexMesh.InnerRadius);
            section.transform.localScale = scale;

            // Orientation
            section.transform.LookAt(direction);

            // Position
            section.transform.position = cell.Position + (direction.ToVector() * (cell.HexMesh.InnerRadius / 2));

            return section;
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

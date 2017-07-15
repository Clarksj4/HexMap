using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public AxialCoordinate Origin;
    public AxialCoordinate End;

    public HexCell[] Cells { get { return cells; } }

    private HexMap map;
    [SerializeField][HideInInspector]
    private HexCell[] cells;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();
    }

    public Pipe Between(HexCell origin, HexCell end)
    {
        Origin = origin.Coordinate;
        End = end.Coordinate;

        cells = new HexCell[] { origin, end };
        foreach (var cell in cells)
            cell.Add(this);

        Vector3 originPosition = cells[0].Position;
        Vector3 endPosition = cells[1].Position;
        Vector3 mid = originPosition + ((endPosition - originPosition) * 0.5f);

        transform.position = mid;
        transform.LookAt(endPosition);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (cells[0].HexMesh.InnerDiameter) / 2);

        return this;
    }

    public Pipe Create()
    {
        // Create a new pipe with same settings as this one
        Pipe newPipe = Instantiate(this);

        // Give the cells it's on a ref to the new pipe
        foreach (var cell in newPipe.Cells)
            cell.Add(newPipe);

        return newPipe;
    }

    public bool IsValidPlacement(HexCell origin, HexCell end)
    {
        return origin != null &&
               end != null &&
               origin != end &&
               origin.IsAdjacent(end) &&
               !origin.HasPipeTo(end);
    }
}

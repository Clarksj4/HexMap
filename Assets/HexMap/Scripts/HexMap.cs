using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HexMap : MonoBehaviour
{
    public int CellsWide = 2;
    public int CellsDeep = 2;
    public HexCell HexCellPrefab;

    public HexCell this[AxialCoordinate coordinate]
    {
        get
        {
            // Check if map contains cell at given coordinate
            if (!Contains(coordinate))
                return null;

            return cells[coordinate.Z, coordinate.GridColumn];
        }
    }

    private HexCell[,] cells;

    private float OuterRadius { get { return HexCellPrefab.HexMesh.OuterRadius; } }
    private float OuterDiameter { get { return HexCellPrefab.HexMesh.OuterDiameter; } }
    private float InnerRadius { get { return HexCellPrefab.HexMesh.InnerRadius; } }
    private float InnerDiameter { get { return HexCellPrefab.HexMesh.InnerDiameter; } }
    private float Width { get { return (InnerDiameter * CellsWide) + (IsRowOffset * InnerRadius); } }
    private float Depth { get { return OuterDiameter + (OuterRadius * 1.5f * (CellsDeep - 1)); } }
    private int IsRowOffset { get { return CellsDeep > 1 ? 1 : 0; } }
    private Vector3 Size { get { return new Vector3(Width, 0, Depth); } }
    private Vector3 Extents { get { return Size * 0.5f; } }

    private void Awake()
    {
        // Create cell scene objects
        CreateCells();
    }

    public bool Contains(AxialCoordinate coordinate)
    {
        return Contains(coordinate.GridColumn, coordinate.Z);
    }

    private bool Contains(int x, int y)
    {
        return x >= 0 &&
               x < cells.GetLength(1) &&
               y >= 0 &&
               y < cells.GetLength(0);
    }

    private void CreateCells()
    {
        cells = new HexCell[CellsDeep, CellsWide];
        for (int column = 0; column < CellsWide; column++)
        {
            for (int row = 0; row < CellsDeep; row++)
            {
                // Calculate axial coordinate
                AxialCoordinate coordinate = AxialCoordinate.FromGridIndices(column, row);

                // Add cell component, set coordinate, and corresponding position
                HexCell cell = Instantiate(HexCellPrefab, transform) as HexCell;
                cell.Coordinate = coordinate;
                cell.transform.position = GetCellCentre(cell.Coordinate);

                // Save to 2d array
                cells[row, column] = cell;
            }
        }
    }

    public Vector3 GetCellCentre(AxialCoordinate cell)
    {
        // Is the row odd (odd rows are offset by InnerRadius distance)
        int odd = cell.Z % 2;

        // Get cell centre's distance from bottom left of map
        float xFromBottomLeft = InnerRadius + (cell.GridColumn * InnerDiameter) + (odd * InnerRadius);
        float zFromBottomLeft = OuterRadius + (cell.Z * 1.5f * OuterRadius);
        Vector3 fromBottomLeft = new Vector3(xFromBottomLeft, 0, zFromBottomLeft);

        // Convert to distance from centre of map
        Vector3 fromCentre = fromBottomLeft - Extents;

        // Convert to world position
        Vector3 worldPosition = transform.TransformPoint(fromCentre);
        return worldPosition;
    }
}
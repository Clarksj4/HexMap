using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Zenject;

public class HexMap : MonoBehaviour
{
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

    public class Factory : Factory<HexMap>
    {
        [Inject]
        private HexCell.Factory hexCellFactory;

        private int cellsWide;
        private int cellsDeep;
        private HexMap hexMap;

        private float outerRadius;
        private float outerDiameter;
        private float innerRadius;
        private float innerDiameter;
        private int isRowOffset;
        private float width;
        private float depth;
        private Vector3 size;
        private Vector3 extents;

        public HexMap Create(int cellsWide, int cellsDeep, float outerRadius)
        {
            // Params
            this.cellsWide = cellsWide;
            this.cellsDeep = cellsDeep;
            this.outerRadius = outerRadius;

            // Measurements
            outerDiameter = outerRadius * 2;
            innerRadius = outerRadius * HexMetrics.OUTER_TO_INNER_RADIUS_FACTOR;
            innerDiameter = innerRadius * 2;
            isRowOffset = cellsDeep > 1 ? 1 : 0;
            width = (innerDiameter * cellsWide) + (isRowOffset * innerRadius);
            depth = outerDiameter + (outerRadius * 1.5f * (cellsDeep - 1));
            size = new Vector3(width, 0, depth);
            extents = size * 0.5f;

            // Create map
            hexMap = base.Create();
            CreateCells();
            return hexMap;
        }

        private void CreateCells()
        {
            hexMap.cells = new HexCell[cellsDeep, cellsWide];
            for (int column = 0; column < cellsWide; column++)
            {
                for (int row = 0; row < cellsDeep; row++)
                {
                    // Calculate coordinate and position
                    AxialCoordinate coordinate = AxialCoordinate.FromGridIndices(column, row);
                    Vector3 position = GetCellCentre(coordinate);

                    // Create cell and save to map
                    HexCell cell = hexCellFactory.Create(coordinate, position, hexMap);
                    hexMap.cells[row, column] = cell;
                }
            }
        }

        private Vector3 GetCellCentre(AxialCoordinate cell)
        {
            // Is the row odd (odd rows are offset by InnerRadius distance)
            int odd = cell.Z % 2;

            // Get cell centre's distance from bottom left of map
            float xFromBottomLeft = innerRadius + (cell.GridColumn * innerDiameter) + (odd * innerRadius);
            float zFromBottomLeft = outerRadius + (cell.Z * 1.5f * outerRadius);
            Vector3 fromBottomLeft = new Vector3(xFromBottomLeft, 0, zFromBottomLeft);

            // Convert to distance from centre of map
            Vector3 fromCentre = fromBottomLeft - extents;

            // Convert to world position
            Vector3 worldPosition = hexMap.transform.TransformPoint(fromCentre);
            return worldPosition;
        }
    }
}
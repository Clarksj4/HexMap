using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileMap : MonoBehaviour
{
    public int CellsWide;
    public int CellsDeep;

    private Cell[,] cells;

    public abstract void Create();

    public virtual Cell GetCell(int x, int z)
    {
        return cells[x, z];
    }

    public virtual bool Contains(int x, int z)
    {
        bool xContained = x >= 0 && x < CellsWide;
        bool zContained = z >= 0 && z < CellsDeep;
        bool cellExists = cells[x, z] != null;

        return xContained && zContained && cellExists;
    }
}

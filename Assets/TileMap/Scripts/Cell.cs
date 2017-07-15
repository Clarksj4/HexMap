using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cell : MonoBehaviour
{
    public IEnumerable<Cell> Neighbours { get { return neighbours; } }

    private Cell[] neighbours;
    private TileMap map;

    public bool IsAdjacent(Cell other)
    {
        return neighbours.Contains(other);
    }

    private void Awake()
    {
        map = GetComponentInParent<TileMap>();
    }
}

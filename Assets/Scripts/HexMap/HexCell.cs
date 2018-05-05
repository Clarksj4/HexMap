using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Zenject;

public class HexCell : MonoBehaviour
{
    public AxialCoordinate Coordinate;
    public IEnumerable<HexCell> Neighbours { get { return AxialCoordinate.Directions.Select(d => map[Coordinate + d]).Where(c => c != null); } }
    public Vector3 Position { get { return transform.position; } }

    private HexMap map;

    public bool IsAdjacent(HexCell other)
    {
        return Coordinate.IsAdjacent(other.Coordinate);
    }

    public class Factory : Factory<HexCell>
    {
        public HexCell Create(AxialCoordinate coordinate, Vector3 position, HexMap parent)
        {
            HexCell cell = base.Create();
            cell.Coordinate = coordinate;
            cell.transform.SetParent(parent.transform);
            cell.transform.position = position;
            cell.name = coordinate.ToString();
            cell.map = parent;
            return cell;
        }
    }
}
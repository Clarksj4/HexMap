using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    public AxialCoordinate Coordinate;
    public AxialCoordinate Direction;
    public HexCell Cell { get; private set; }

    private HexMap map;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();
    }

    public Node OnAndTowards(HexCell on, HexCell towards)
    {
        Coordinate = on.Coordinate;
        Cell = on;
        Direction = towards.Coordinate - Coordinate;

        // Set position
        transform.position = Cell.Position;

        // Set rotation
        float rotation = 30 + AxialCoordinate.Directions.TakeWhile(d => d != Direction).Sum(d => 60f);
        transform.rotation = Quaternion.Euler(0, rotation, 0);

        return this;
    }

    public Node Create()
    {
        // Create a new node with same settings as this one
        Node newNode = Instantiate(this);

        // Give the cell a ref to the new node
        Cell.Add(newNode);

        return newNode;
    }

    public bool IsValidPlacement(HexCell cell)
    {
        return !cell.HasNode();
    }
}

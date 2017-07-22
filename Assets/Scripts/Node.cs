using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    public AxialCoordinate Coordinate
    {
        get { return coordinate; }
        set { Cell = map[coordinate]; }
    }

    public HexCell Cell
    {
        get { return cell; }
        set
        {
            // Remove self from current cell
            cell.Remove(this);

            // Update coordinate and cell
            cell = value;
            coordinate = cell.Coordinate;

            // Move to new cell and add self
            transform.position = cell.Position;
            cell.Add(this);
        }
    }

    public AxialCoordinate Direction
    {
        get { return direction; }
        set
        {
            direction = value;

            // Set rotation
            float rotation = 30 + AxialCoordinate.Directions.TakeWhile(d => d != Direction).Sum(d => 60f);
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }


    [SerializeField]
    private AxialCoordinate coordinate;
    [SerializeField]
    private AxialCoordinate direction;
    [SerializeField][HideInInspector]
    private HexCell cell;
    [SerializeField][HideInInspector]
    private HexMap map;

    private void Awake()
    {
        map = GetComponentInParent<HexMap>();
    }

    public bool IsValidPlacement(HexCell cell)
    {
        return cell != null &&
               !cell.HasNode();
    }

    //
    // Builder Methods
    //

    /// <summary>
    /// Places this node at the given coordinate. DOES NOT set the cells node reference to this node.
    /// </summary>
    public Node At(AxialCoordinate coordinate)
    {
        return At(map[coordinate]);
    }

    /// <summary>
    /// Places this node at the given cell. DOES NOT set the cells node reference to this node.
    /// </summary>
    public Node At(HexCell cell)
    {
        // Update references
        this.cell = cell;
        coordinate = cell.Coordinate;

        // Move to cell's position
        transform.position = cell.Position;

        return this;
    }

    /// <summary>
    /// Rotates this node so it is facing the given direction
    /// </summary>
    public Node Towards(AxialCoordinate direction)
    {
        Direction = direction;
        return this;
    }

    /// <summary>
    /// Instantiates a copy of this cell. The cell under this node has its node reference set to the newly created node
    /// </summary>
    public Node Create()
    {
        // Only create a new node if the underlying cell has no node
        if (IsValidPlacement(cell))
        {
            // Create a new node with same settings as this one
            Node newNode = Instantiate(this);

            // Give the cell a ref to the new node
            Cell.Add(newNode);

            return newNode;
        }

        throw new InvalidOperationException("Cannot create node on a cell where one already exists");
    }
}

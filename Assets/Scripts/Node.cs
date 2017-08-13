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

    public HexDirection Direction
    {
        get { return direction; }
        set
        {
            // Set rotation
            direction = value;
            transform.LookAt(direction);
        }
    }

    public Area Output { get { return output; } }

    public Player player;
    public float outputAmount = 0.1f;
    [SerializeField]
    private AxialCoordinate coordinate;
    [SerializeField]
    private HexDirection direction;
    [SerializeField][HideInInspector]
    private HexCell cell;
    [SerializeField][HideInInspector]
    private HexMap map;
    [SerializeField]
    private Area output;

    private Coroutine outputting;

    private void Awake()
    {
        map = GetComponentInParent<HexMap>();
        player = FindObjectOfType<Player>();
    }

    public bool IsValidPlacement(HexCell cell)
    {
        return cell != null &&
               !cell.HasNode;
    }

    public void BeginOutput()
    {
        if (outputting == null)
            StartCoroutine(DoOutput());
    }

    public void StopOutputting()
    {
        StopCoroutine(outputting);
        outputting = null;
    }

    IEnumerator DoOutput()
    {
        while (true)
        {
            IEnumerable<AxialCoordinate> coords = output.From(Coordinate, HexRotation.FromDirection(Direction));
            int tiers = coords.Last().Distance(Coordinate) + 1;

            foreach (var coordinate in coords)
            {
                HexCell cell = map[coordinate];

                if (cell)
                {
                    float tier = coordinate.Distance(Coordinate);
                    float t = 1 - (tier / tiers);

                    cell.IncrementControl(player, outputAmount * t);
                }
            }

            yield return new WaitForSeconds(1);
        }
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
    public Node Towards(HexDirection direction)
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
            newNode.map = newNode.GetComponentInParent<HexMap>();

            cell.RemoveControl();
            cell.SetControl(FindObjectOfType<Player>(), 1);

            return newNode;
        }

        throw new InvalidOperationException("Cannot create node on a cell where one already exists");
    }
}

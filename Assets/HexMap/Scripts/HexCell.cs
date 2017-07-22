﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class HexCell : MonoBehaviour
{
    private const float DEFAULT_PUNCH_Y_SCALE = 0.5f;
    private const float DEFAULT_PUNCH_Y_TIME = 3;

    public AxialCoordinate Coordinate;
    public IEnumerable<HexCell> Neighbours { get { return AxialCoordinate.Directions.Select(d => map[Coordinate + d]); } }
    public HexMesh HexMesh { get { return GetComponentInChildren<HexMesh>(); } }
    public Vector3 Position { get { return transform.position; } }

    private HexMap map;

    public Node Node { get; private set; }
    public Pipe Pipe { get; private set; }
    public bool HasNode { get { return Node != null; } }
    public bool HasPipe { get { return Pipe != null; } }

    public void PunchYPosition(float scale, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * scale, time);
    }

    public void Quake(float dY, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * dY, time);
    }

    public bool Add(Pipe pipe)
    {
        bool added = false;

        // Don't add if already contains
        if (!HasPipe)
        {
            Pipe = pipe;
            added = true;
        }

        return added;
    }

    public bool Add(Node node)
    {
        bool added = false;

        // Don't add node if already has one
        if (!HasNode)
        {
            Node = node;
            added = true;
        }

        return added;
    }

    public bool Remove(Node node)
    {
        bool removed = false;

        if (Node == node)
        {
            Node = null;
            removed = true;
        }

        return removed;
    }

    public bool IsAdjacent(HexCell other)
    {
        return Coordinate.IsAdjacent(other.Coordinate);
    }

    //public bool HasPipeTo(HexCell other)
    //{
    //    return Pipe.Where(p => p.Cells.Contains(other) && p.Cells.Contains(this)).Any();
    //}

    private void Awake()
    {
        map = GetComponentInParent<HexMap>();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.Label(transform.position, Coordinate.ToString());
    }
}
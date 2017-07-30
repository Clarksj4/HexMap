using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class HexCell : MonoBehaviour
{
    public AxialCoordinate Coordinate;
    public IEnumerable<HexCell> Neighbours { get { return AxialCoordinate.Directions.Select(d => map[Coordinate + d]).Where(c => c != null); } }
    public HexMesh HexMesh { get { return GetComponentInChildren<HexMesh>(); } }
    public Vector3 Position { get { return transform.position; } }
    public Node Node { get; private set; }
    public Pipe Pipe { get; private set; }
    public bool HasNode { get { return Node != null; } }
    public bool HasPipe { get { return Pipe != null; } }

    private HexMap map;
    private Coroutine quaking;

    public bool Add(Pipe pipe)
    {
        bool added = false;

        // Don't add if already contains
        if (!HasPipe)
        {
            Pipe = pipe;
            pipe.transform.SetParent(transform);
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
            node.transform.SetParent(transform);
            added = true;
        }

        return added;
    }

    public bool Remove(Node node)
    {
        bool removed = false;

        if (Node == node)
        {
            Node.transform.SetParent(null);
            Node = null;
            removed = true;
        }

        return removed;
    }

    public bool IsAdjacent(HexCell other)
    {
        return Coordinate.IsAdjacent(other.Coordinate);
    }

    public void Quake(float dY, float time, float delay = 0)
    {
        if (quaking == null)
            quaking = StartCoroutine(DoQuake(dY, time, delay));
    }

    private void Awake()
    {
        map = GetComponentInParent<HexMap>();
    }

    private void Start()
    {
        name = Coordinate.ToString();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.Label(transform.position, Coordinate.ToString());
    }

    IEnumerator DoQuake(float dY, float duration, float delay)
    {
        // Wait for delay, then quake
        yield return new WaitForSeconds(delay);
        iTween.PunchPosition(gameObject, Vector3.down * dY, duration);

        // Wait for quake's duration to expire, then null ref to quake
        yield return new WaitForSeconds(duration);
        quaking = null;
    }
}
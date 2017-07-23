using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class HexCell : MonoBehaviour
{
    private const float DEFAULT_PUNCH_Y_SCALE = 0.5f;
    private const float DEFAULT_PUNCH_Y_TIME = 3;
    private const float QUAKE_PROPOGATION_DELAY = 0.1f;
    private const float QUAKE_INTENSITY_FALLOFF = 0.3f;

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
    
    public void PunchYPosition(float scale, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * scale, time);
    }

    public void Quake(float dY, float time)
    {
        if (dY > QUAKE_INTENSITY_FALLOFF && 
            time > QUAKE_PROPOGATION_DELAY && 
            quaking == null)
            quaking = StartCoroutine(DoQuake(dY, time));
    }

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

    IEnumerator DoQuake(float dY, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * dY, time);

        yield return new WaitForSeconds(QUAKE_PROPOGATION_DELAY);

        float timeRemaining = time - QUAKE_PROPOGATION_DELAY;
        float intensityRemaining = dY * (1 - QUAKE_INTENSITY_FALLOFF);

        foreach (var neighbour in Neighbours)
            neighbour.Quake(intensityRemaining, timeRemaining);

        yield return new WaitForSeconds(timeRemaining);

        quaking = null;
    }
}
using System.Collections;
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

    private HexMap map;
    private ClickableMesh clickable;
    private bool highlight;

    // Nodes
    // Pipes
    // Controller

    public void PunchYPosition()
    {
        PunchYPosition(DEFAULT_PUNCH_Y_SCALE, DEFAULT_PUNCH_Y_TIME);
    }

    public void PunchYPosition(float scale, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * scale, time);
    }

    public bool IsAdjacent(HexCell other)
    {
        return Coordinate.Adjacent(other.Coordinate);
    }

    public void Highlight(bool active)
    {
        highlight = active;
    }

    void Awake()
    {
        map = GetComponentInParent<HexMap>();
        clickable = GetComponentInChildren<ClickableMesh>();
    }

    void Update()
    {
        if (clickable.Clicked)
            PunchYPosition();
    }

    private void OnDrawGizmos()
    {
        if (highlight)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireMesh(HexMesh.SharedMesh, 0, transform.GetChild(0).position);
            Gizmos.color = Color.white;
        }

        Handles.color = Color.red;
        Handles.Label(transform.position, Coordinate.ToString());
    }
}
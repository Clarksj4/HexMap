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

    public float OuterRadius;
    public float OuterDiameter { get { return OuterRadius * 2; } }
    public float InnerRadius { get { return OuterRadius * HexMetrics.OUTER_TO_INNER_RADIUS_FACTOR; } }
    public float InnerDiameter { get { return InnerRadius * 2; } }

    private HexMap map;

    public void PunchYPosition()
    {
        PunchYPosition(DEFAULT_PUNCH_Y_SCALE, DEFAULT_PUNCH_Y_TIME);
    }

    public void PunchYPosition(float scale, float time)
    {
        iTween.PunchPosition(gameObject, Vector3.down * scale, time);
    }

    private void OnMouseDown()
    {
        PunchYPosition();
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.Label(transform.position, Coordinate.ToString());
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public AxialCoordinate Origin;
    public AxialCoordinate End;
    public AxialCoordinate Direction { get { return End - Origin; } }

    private HexMap map;

    private void Awake()
    {
        map = FindObjectOfType<HexMap>();
    }

    // Use this for initialization
    void Start ()
    {
        Vector3 originPosition = map.GetCellCentre(Origin);
        Vector3 endPosition = map.GetCellCentre(End);
        Vector3 mid = originPosition + ((endPosition - originPosition) * 0.5f);

        transform.position = mid;
        transform.LookAt(endPosition);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, (map[Origin].HexMesh.InnerDiameter) / 2);
	}
}

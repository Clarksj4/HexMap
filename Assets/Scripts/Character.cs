using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public HexCell Cell;
    public AxialCoordinate Coordinate
    {
        get { return coordinate; }
        set
        {
            coordinate = value;
            if (map != null)
                transform.position = map.GetCellCentre(coordinate);
        }
    }
    [SerializeField]
    private AxialCoordinate coordinate;

    private HexMap map;

	// Use this for initialization
	void Start ()
    {
        map = FindObjectOfType<HexMap>();
        // Set cell to cell at coordinate
        // Set position to cell top position
        Coordinate = coordinate;

	}

    private void OnValidate()
    {
        Coordinate = coordinate;
    }
}

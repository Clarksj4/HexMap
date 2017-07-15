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

	// Use this for initialization
	void Start ()
    {
        map = FindObjectOfType<HexMap>();
        Cell = map[Coordinate];

        transform.position = Cell.transform.position;

        float rotation = 30 + AxialCoordinate.Directions.TakeWhile(d => d != Direction).Sum(d => 60f);
        transform.Rotate(Vector3.up, rotation);
	}
}

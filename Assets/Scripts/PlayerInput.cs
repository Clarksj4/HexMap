using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // If left mouse down
    // Get cell
    // if different to current cell
    // Add pipe between cells
    public Pipe pipePrefab;

    HexCell previousCell;

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(cameraRay, out hit, 100))
            {
                HexCell cell = hit.collider.GetComponentInParent<HexCell>();
                if (previousCell != null && previousCell != cell)
                {
                    Pipe pipe = Instantiate(pipePrefab);
                    pipe.Origin = cell.Coordinate;
                    pipe.Direction = previousCell.Coordinate - cell.Coordinate;
                    pipe.Length = 1;
                }
                previousCell = cell;
            }
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    // If left mouse down
    // Get cell
    // if different to current cell
    // Add pipe between cells
    public Pipe PipePrefab;
    public Node NodePrefab;

    HexCell previousCell;

	// Update is called once per frame
	void Update ()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 100))
        {
            HexCell cell = hit.collider.GetComponentInParent<HexCell>();

            HighlightCell(cell);

            if (Input.GetMouseButton(0))
            {
                if (previousCell != null && previousCell != cell && previousCell.IsAdjacent(cell))
                {
                    Pipe pipe = Instantiate(PipePrefab);
                    pipe.Origin = previousCell.Coordinate;
                    pipe.End = cell.Coordinate;
                }
            }

            else if (Input.GetMouseButton(1))
            {
                Node node = Instantiate(NodePrefab);
                node.Coordinate = cell.Coordinate;
                node.Direction = AxialCoordinate.UpRight;
            }

            previousCell = cell;
        }
	}

    void HighlightCell(HexCell cell)
    {
        cell.Highlight(true);
        if (previousCell != null && previousCell != cell)
            previousCell.Highlight(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BuildState
{ 
    Pipe, Node, None
}

public class PlayerInput : MonoBehaviour
{
    public Pipe PipePrefab;
    public Node NodePrefab;
    public Material correctPlacement;
    public Material incorrectPlacement;

    private HexCell currentCell;
    private HexCell previousCell;
    private BuildState state;
    private GameObject template;

    public void SetState(int newState)
    {
        SetState((BuildState)newState);
    }

    public void SetState(BuildState newState)
    {
        if (newState != state)
        {
            // Cancel other states, remove template, enter new state
            StopAllCoroutines();
            Destroy(template);
            state = newState;

            switch (state)
            {
                case BuildState.Pipe:
                    StartCoroutine(PlacePipeState());
                    break;
                case BuildState.Node:
                    StartCoroutine(PlaceNodeState());
                    break;
                case BuildState.None:
                default:
                    break;
            }
        }
    }

    private void Awake()
    {
        SetState(BuildState.None);
    }

    private void Update ()
    {
        previousCell = currentCell;
        currentCell = GetCellUnderCursor();
	}

    private HexCell GetCellUnderCursor()
    {
        HexCell cell = null;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 100))
            cell = hit.collider.GetComponentInParent<HexCell>();

        return cell;
    }

    private void HighlightTemplate(bool validPlacement)
    {
        if (validPlacement)
            HighlightTemplate(correctPlacement);
        else
            HighlightTemplate(incorrectPlacement);
    }

    private void HighlightTemplate(Material newMaterial)
    {
        // Set all materials for all renderers to given material
        foreach (var renderer in template.GetComponentsInChildren<Renderer>())
            renderer.materials = Enumerable.Repeat(newMaterial, renderer.materials.Length).ToArray();
    }

    IEnumerator PlacePipeState()
    {
        // Instantiate a pipe at cursor position
        // Check if placement is appropriate
        // Colour it appropriately
        // Check for clicks

        template = Instantiate(PipePrefab).gameObject;

        bool validPlacement = false;

        while (true)
        {
            // If a new cell is moused over
            if (previousCell != currentCell)
            {
                // Move template to new position
                Pipe templatePipe = template.GetComponent<Pipe>();
                templatePipe.Between(previousCell, currentCell);

                // Check if new cell is a valid place to build pipe
                validPlacement = templatePipe.IsValidPlacement(previousCell, currentCell);
                HighlightTemplate(validPlacement);
            }

            // If left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Is pipe placement valid?
                if (validPlacement)
                    PipePrefab.Between(previousCell, currentCell).Create();

                else
                {
                    // PunchY the template pipe
                    // Play error sound
                }
            }

            // Repeat next frame
            yield return null;
        }
    }

    IEnumerator PlaceNodeState()
    {
        // Instantiate a node at cursor position
        // Check if placement is appropriate
        // Colour it appropriately
        // Check for clicks

        template = Instantiate(NodePrefab).gameObject;

        bool validPlacement = false;

        while (true)
        {
            if (previousCell != null && currentCell != null)
            {
                // If a new cell is moused over
                if (previousCell != currentCell)
                {
                    // Move template to new position
                    Node templateNode = template.GetComponent<Node>();
                    templateNode.OnAndTowards(currentCell, previousCell);

                    // Check if new cell is a valid place to build pipe
                    validPlacement = templateNode.IsValidPlacement(currentCell);
                    HighlightTemplate(validPlacement);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    // Is node placement valid?
                    if (validPlacement)
                    {
                        NodePrefab.OnAndTowards(currentCell, previousCell).Create();
                        SetState(BuildState.None);
                    }

                    else
                    {
                        // PunchY the template node
                        // Play error sound
                    }
                }
            }

            // Repeat next frame
            yield return null;
        }
    }
}

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
    public AudioClip snapToSound;
    public AudioClip rotateSound;

    private HexCell currentCell;
    private HexCell previousCell;
    private BuildState state;

    private Node templateNode;

    public void SetState(int newState)
    {
        SetState((BuildState)newState);
    }

    public void SetState(BuildState newState)
    {
        if (newState != state)
        {
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

    private HexCell GetCellUnderCursor()
    {
        HexCell cell = null;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 100))
            cell = hit.collider.GetComponentInParent<HexCell>();

        return cell;
    }

    private void HighlightTemplate(GameObject template, bool validPlacement)
    {
        if (validPlacement)
            HighlightTemplate(template, correctPlacement);
        else
            HighlightTemplate(template, incorrectPlacement);
    }

    private void HighlightTemplate(GameObject template, Material newMaterial)
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

        //template = Instantiate(PipePrefab).gameObject;

        bool validPlacement = false;

        while (true)
        {
            previousCell = currentCell;
            currentCell = GetCellUnderCursor();

            // If a new cell is moused over
            if (previousCell != currentCell)
            {
                if (currentCell.HasPipe)
                {
                    // Check if cells are adjacent
                    // Get direction
                    // add pipe section
                }

                // Move template to new position
                //Pipe templatePipe = template.GetComponent<Pipe>();
                //templatePipe.Between(previousCell, currentCell);

                // Check if new cell is a valid place to build pipe
                //validPlacement = templatePipe.IsValidPlacement(previousCell, currentCell);
                //HighlightTemplate(validPlacement);
            }

            // If left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                // Is pipe placement valid?
                if (validPlacement) { }
                //PipePrefab.Between(previousCell, currentCell).Create();

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
        // Template node highlighting where the actual node will be built
        templateNode = Instantiate(NodePrefab);
        templateNode.Towards(AxialCoordinate.Directions[0]);
        templateNode.gameObject.SetActive(false);               // Hide template until it is positioned on a cell

        bool validPlacement = false;
        int rotationIncrement = 0;

        // While in BuildNodeState
        while (state == BuildState.Node)
        {
            // Get ref to cell the cursor is currently over as well as the previous one
            previousCell = currentCell;
            currentCell = GetCellUnderCursor();
            
            // If there is a cell bing targeted
            if (currentCell != null)
            {
                // Show template when its on a cell
                templateNode.gameObject.SetActive(true);

                // Rotate if Q or E are pressed, play sound
                rotationIncrement = HandleRotationInput(templateNode, rotationIncrement);

                // Move if cursor is over a new cell, play sound
                validPlacement = HandleNodeMovementInput(templateNode, validPlacement);

                // Build node if at mouse clicked at valid position
                HandleNodePlacementInput(templateNode, validPlacement);
            }

            // Repeat next frame
            yield return null;
        }

        Destroy(templateNode.gameObject);
    }

    private bool HandleNodeMovementInput(Node templateNode, bool validPlacement)
    {
        // If a new cell is moused over
        if (previousCell != currentCell &&
            templateNode.Cell != currentCell)
        {
            // Move template to new position
            templateNode.At(currentCell);

            AudioSource source = templateNode.GetComponent<AudioSource>();
            PlaySound(source, snapToSound);

            // Check if new cell is a valid place to build pipe
            validPlacement = templateNode.IsValidPlacement(currentCell);
            HighlightTemplate(templateNode.gameObject, validPlacement);
        }

        return validPlacement;
    }

    private void HandleNodePlacementInput(Node templateNode, bool validPlacement)
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Is node placement valid?
            if (validPlacement)
            {
                // Create node at current cell, exit build state
                NodePrefab.At(currentCell).Towards(templateNode.Direction).Create();
                SetState(BuildState.None);
            }

            else
            {
                // PunchY the template node
                // Play error sound
            }
        }
    }

    private int HandleRotationInput(Node templateNode, int rotationIncrement)
    {
        // Rotate node with Q and E
        bool clockwise = Input.GetKeyDown(KeyCode.E);
        bool antiClockwise = Input.GetKeyDown(KeyCode.Q);

        if (clockwise || antiClockwise)
        {
            // Increment and wrap rotationIncrement
            rotationIncrement += Maths.Sign(clockwise);
            rotationIncrement = Maths.Wrap(rotationIncrement, 0, AxialCoordinate.Directions.Length);
            templateNode.Direction = AxialCoordinate.Directions[rotationIncrement];

            // Play rotation sound
            AudioSource source = templateNode.GetComponent<AudioSource>();
            PlaySound(source, rotateSound, clockwise);
        }
        
        return rotationIncrement;
    }

    private void PlaySound(AudioSource source, AudioClip clip, bool forwards = true)
    {
        source.clip = clip;
        
        source.timeSamples = forwards ? 0 : clip.samples - 1;
        source.pitch = Maths.Sign(forwards);

        source.Play();
    }
}

using System.IO;
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
    public Quake quake;

    public Pipe PipePrefab;
    public Node NodePrefab;
    public Material correctPlacement;
    public Material incorrectPlacement;
    public AudioClip snapToSound;
    public AudioClip rotateSound;
    public float NodeShakeScale = 0.5f;
    public float NodeShakeDuration = 1f;

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

        //string lineJson = JsonUtility.ToJson(quakeLine);
        //StreamWriter writer = new StreamWriter(Application.dataPath + "/line.json");
        //writer.Write(lineJson);
        //writer.Close();
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
        while (true)
        {
            previousCell = currentCell;
            currentCell = GetCellUnderCursor();

            // If a new cell is moused over
            if (currentCell != null && 
                previousCell != null && 
                currentCell.IsAdjacent(previousCell) &&
                Input.GetMouseButton(0))
            {
                Pipe pipe = currentCell.Pipe;
                Pipe adjacentPipe = previousCell.Pipe;

                if (!pipe)
                {
                    pipe = Instantiate(PipePrefab);
                    pipe.Coordinate = currentCell.Coordinate;
                }

                if (!adjacentPipe)
                {
                    adjacentPipe = Instantiate(PipePrefab);
                    adjacentPipe.Coordinate = previousCell.Coordinate;
                }

                HexDirection direction = pipe.Coordinate.GetDirection(adjacentPipe.Coordinate);
                pipe.AddSection(direction);
                adjacentPipe.AddSection(direction.Opposite());
            }

            // Repeat next frame
            yield return null;
        }
    }

    IEnumerator PlaceNodeState()
    {
        // Template node highlighting where the actual node will be built
        templateNode = Instantiate(NodePrefab);
        templateNode.Towards(0);
        templateNode.gameObject.SetActive(false);               // Hide template until it is positioned on a cell

        bool validPlacement = false;
        HexDirection rotation = 0;

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
                rotation = HandleRotationInput(templateNode, rotation);

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
                Node newNode = NodePrefab.At(currentCell).Towards(templateNode.Direction).Create();
                newNode.BeginOutput();
                SetState(BuildState.None);

                quake.DoAt(currentCell.Coordinate, HexRotation.FromCoordinates(previousCell.Coordinate, currentCell.Coordinate));
            }

            else
            {
                // Shake the template node
                iTween.ShakePosition(templateNode.gameObject, HexMetrics.ORIENTATION * NodeShakeScale, NodeShakeDuration);
                
                // Play error sound
            }
        }
    }

    private HexDirection HandleRotationInput(Node templateNode, HexDirection rotation)
    {
        // Rotate node with Q and E
        bool clockwise = Input.GetKeyDown(KeyCode.E);
        bool antiClockwise = Input.GetKeyDown(KeyCode.Q);

        if (clockwise || antiClockwise)
        {
            // Increment and wrap rotationIncrement
            rotation += Maths.Sign(clockwise);
            rotation = (HexDirection)Maths.Wrap((int)rotation, 0, AxialCoordinate.Directions.Length);
            templateNode.Direction = (HexDirection)rotation;

            // Play rotation sound
            AudioSource source = templateNode.GetComponent<AudioSource>();
            PlaySound(source, rotateSound, clockwise);
        }
        
        return rotation;
    }

    private void PlaySound(AudioSource source, AudioClip clip, bool forwards = true)
    {
        source.clip = clip;
        
        source.timeSamples = forwards ? 0 : clip.samples - 1;
        source.pitch = Maths.Sign(forwards);

        source.Play();
    }
}

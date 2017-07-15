using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class HexMesh : MonoBehaviour
{
    public float Height = 1;
    public float OuterRadius = 1;
    public float OuterDiameter { get { return OuterRadius * 2; } }
    public float InnerRadius { get { return OuterRadius * HexMetrics.OUTER_TO_INNER_RADIUS_FACTOR; } }
    public float InnerDiameter { get { return InnerRadius * 2; } }

    /// <summary>
    /// First 7 verts in mesh are the top face of the hex column (includes central vert)
    /// </summary>
    public IEnumerable<Vector3> TopFaceVerts { get { return Mesh.vertices.Take(7); } }
    public Mesh Mesh { get { return meshFilter.mesh; } }
    public Mesh SharedMesh { get { return meshFilter.sharedMesh; } }

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void Create()
    {
        meshFilter.mesh = MeshCreate.HexColumn(Vector3.zero, Height, Vector3.up, OuterRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HexMesh
{
    /// <summary>
    /// Creates a mesh consisting of a hexagonal top and bottom facing and six additional rectangular side faces.
    /// </summary>
    public static Mesh Create3DHexColumn(Vector3 centre, float height, Vector3 normal, float hexSideLength)
    {
        // Calculate centre of bottom and top hexes
        Vector3 topCentre = centre + (normal.normalized * (height * 0.5f));
        Vector3 bottomCentre = centre - (normal.normalized * (height * 0.5f));

        // Create top and bottom hexes, and 6 side quads
        Mesh top = Create2DHexMesh(topCentre, normal, hexSideLength);
        Mesh bottom = Create2DHexMesh(bottomCentre, -normal, hexSideLength);
        Mesh sides = Create3DHexSides(topCentre, bottomCentre, normal, hexSideLength);

        // Combine meshes into one
        CombineInstance[] combine = new CombineInstance[]
        {
            new CombineInstance() { mesh = top },
            new CombineInstance() { mesh = bottom },
            new CombineInstance() { mesh = sides }
        };

        Mesh hexColumn = new Mesh();
        hexColumn.name = "HexColumn";

        // Submeshes are not combined, so top, bottom, and sides can all use different materials
        hexColumn.CombineMeshes(combine, false, false, false);

        return hexColumn;
    }

    /// <summary>
    /// Creates a single sided, flat, hexagonal mesh
    /// </summary>
    public static Mesh Create2DHexMesh(Vector3 centre, Vector3 normal, float sideLength)
    {
        // Mesh params
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        // Verts
        verts.Add(centre);
        verts.AddRange(GetHexVerts(centre, normal, sideLength));

        // Normals all face the same direction
        normals = Enumerable.Repeat(normal, 7).ToList();

        // Tris
        for (int i = 1; i < verts.Count - 1; i++)
        {
            tris.Add(0);
            tris.Add(i);
            tris.Add(i + 1);
        }

        tris.Add(0);
        tris.Add(verts.Count - 1);
        tris.Add(1);

        // TODO: uvs

        // Build mesh
        Mesh mesh = new Mesh();
        mesh.name = "HexMesh";
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        return mesh;
    }

    /// <summary>
    /// Gets 6 vertices arranged in a hexagonal pattern around the given centre point
    /// </summary>
    public static Vector3[] GetHexVerts(Vector3 centre, Vector3 normal, float sideLength)
    {
        // Verts are by default oriented with an upwards normal vector, this quaternion will rotate points to have the given normal
        Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal.normalized);

        // Each angle of hex is 60 degrees, each point is incrementally rotated this amount about the given vector
        Vector3[] points = new Vector3[6];
        for (int i = 0; i < points.Length; i++)
        {
            // Hex angle is 60 degrees
            Quaternion rotation = Quaternion.AngleAxis(i * 60, normal);
            Vector3 direction = rotation * normalRotation * Vector3.forward;

            // Push point out from centre of hex by sideLength distance
            Vector3 point = centre + (direction * sideLength);
            points[i] = point;
        }

        return points;
    }

    /// <summary>
    /// Creates the 6 rectangle side meshes of a 3D hex column
    /// </summary>
    private static Mesh Create3DHexSides(Vector3 topCentre, Vector3 bottomCentre, Vector3 normal, float sideLength)
    {
        Vector3[] topVerts = GetHexVerts(topCentre, normal, sideLength);
        Vector3[] bottomVerts = GetHexVerts(bottomCentre, -normal, sideLength);

        // Each side is a quad consisting of two triangles
        List<Mesh> quads = new List<Mesh>();
        for (int i = 0; i < 6; i++)
        {
            Vector3[] quadPoints = new Vector3[] 
            {
                topVerts[i],
                topVerts[(i + 1) % 6],

                // Bottom verts are ordered in the opposite direction to the top verts
                bottomVerts[GetBottomHexVertIndex(i + 1)],
                bottomVerts[GetBottomHexVertIndex(i)]
            };

            // Verts are by default oriented with an upwards normal vector, this quaternion will rotate points to have the given normal
            Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal.normalized);
            
            // Hex angle is 60 degrees, the extra 30 degrees points the direction vector towards the middle of a hex side
            Quaternion rotation = Quaternion.AngleAxis(30 + i * 60, normal);
            Vector3 quadNormal = rotation * normalRotation * Vector3.forward;

            Mesh quad = CreateQuad(quadPoints, quadNormal);
            quads.Add(quad);
        }

        // Combine the six sides into a single mesh
        CombineInstance[] combine = quads.Select(q => new CombineInstance() { mesh = q }).ToArray();

        Mesh sides = new Mesh();
        sides.name = "Hex Mesh";
        sides.CombineMeshes(combine, true, false, false);

        return sides;
    }

    /// <summary>
    /// Creates a rectangular mesh from the four given points, oriented with the given normal
    /// </summary>
    private static Mesh CreateQuad(Vector3[] points, Vector3 normal)
    {
        // Mesh params
        List<int> tris = new List<int>();
        List<Vector3> normals = Enumerable.Repeat(normal, 4).ToList();
        List<Vector2> uvs = new List<Vector2>();

        // Quad consists of two triangles
        tris.Add(0);
        tris.Add(2);
        tris.Add(1);

        tris.Add(0);
        tris.Add(3);
        tris.Add(2);

        // TODO: uvs

        Mesh mesh = new Mesh();
        mesh.name = "Quad";
        mesh.vertices = points;
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);

        return mesh;
    }

    /// <summary>
    /// Gets the bottom hex column vert that is aligned with the given top vert index
    /// </summary>
    private static int GetBottomHexVertIndex(int topHexVertIndex)
    {
        int index = -topHexVertIndex + 3;

        // Wrap the index
        return index < 0 ? index + 6 : index;
    }
}


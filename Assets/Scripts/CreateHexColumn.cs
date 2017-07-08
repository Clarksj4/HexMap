using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateHexColumn : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float side = 1;
    public float height = 1;

    private void Start()
    {
        Mesh hexColumn = HexMesh.Create3DHexColumn(Vector3.zero, height, direction, side);
        AssetDatabase.CreateAsset(hexColumn, "Assets/HexColumn.asset");
        AssetDatabase.SaveAssets();
    }
}

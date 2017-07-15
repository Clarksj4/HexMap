using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateAsset : MonoBehaviour
{
    public Vector3 direction = Vector3.up;
    public float side = 1;
    public float height = 1;

    private void Start()
    {
        Mesh hex = MeshCreate.HexPlane(Vector3.zero, direction, side);
        AssetDatabase.CreateAsset(hex, "Assets/Hex.asset");
        AssetDatabase.SaveAssets();
    }
}

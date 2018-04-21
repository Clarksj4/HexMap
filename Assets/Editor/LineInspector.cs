using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{
    SerializedProperty hexRotation;
    SerializedProperty range;

    void OnEnable()
    {
        hexRotation = serializedObject.FindProperty("Rotation");
        range = serializedObject.FindProperty("Range");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(range);
        EditorGUILayout.PropertyField(hexRotation);
        serializedObject.ApplyModifiedProperties();
    }
}
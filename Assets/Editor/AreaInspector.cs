using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Area))]
public class AreaInspector : Editor
{
    SerializedProperty shapes;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        Area area = (Area)target;

        SerializedProperty shapes = serializedObject.FindProperty("Shapes");
        Debug.Log(shapes.name);
        shapes.Next(true);
        Debug.Log(shapes.arraySize);
        Debug.Log(area.Shapes.Count);
        ShowArrayProperty(shapes);

        if (GUILayout.Button("Add Line"))
        {
            Line line = CreateInstance<Line>();
            area.Add(line);
            AssetDatabase.AddObjectToAsset(line, area);
            EditorUtility.SetDirty(area);
        }

        if (GUILayout.Button("Add Ring"))
        {
            area.Add(CreateInstance<Ring>());
            EditorUtility.SetDirty(area);
        }
    }

    public void ShowArrayProperty(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
            new GUIContent("Bla" + (i + 1).ToString()));
        }
        EditorGUI.indentLevel -= 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class LinePropertyDrawer : PropertyDrawer
{
    private const float LINE_LABEL_WIDTH = 35;
    private const float POST_LABEL_SPACING = 5;
    private const float RANGE_LABEL_WIDTH = 55;


    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        Line line = (Line)property.objectReferenceValue;

        Rect labelRect = new Rect(rect.x, rect.y, LINE_LABEL_WIDTH, rect.height);
        EditorGUI.LabelField(rect, "Line:");

        // Remaining space divided in two (half for each property)
        float halfPropertySpace = (rect.width - LINE_LABEL_WIDTH) / 2;

        float rangePropertyStart = rect.x + LINE_LABEL_WIDTH + POST_LABEL_SPACING;
        Rect rangeRect = new Rect(rangePropertyStart, rect.y, halfPropertySpace, rect.height);
        GUIContent rangeLabel = new GUIContent("Rng");
        Rect rangeLabelRect = EditorGUI.PrefixLabel(rangeRect, rangeLabel);
        Rect rangeValueRect = new Rect(rangePropertyStart + rangeLabelRect.width, rect.y, rangeRect.width - rangeLabelRect.width, rect.height);
        line.Range = EditorGUI.IntField(rangeValueRect, line.Range);

        float rotationPropertyStart = rangeValueRect.x + rangeValueRect.width;
        Rect rotationRect = new Rect(rotationPropertyStart, rect.y, halfPropertySpace, rect.height);
        line.Rotation.Degrees = (HexRotationDegrees)EditorGUI.EnumPopup(rotationRect, line.Rotation.Degrees);
    }
}

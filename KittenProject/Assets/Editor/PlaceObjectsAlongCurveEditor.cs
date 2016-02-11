using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(PlaceObjectsAlongCurve))]
public class PlaceObjectsAlongCurveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PlaceObjectsAlongCurve placer = (PlaceObjectsAlongCurve)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Spawn"))
        {
            placer.Spawn();
        }

        if (GUILayout.Button("Remove all objects"))
        {
            placer.RemoveAll();
        }
    }
}

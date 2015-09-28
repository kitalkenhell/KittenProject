using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CubicHermiteCurve))]
public class CubicHermiteCurveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CubicHermiteCurve curve = (CubicHermiteCurve)target;

        if (GUILayout.Button("Reset"))
        {
            curve.Reset();
        }
    }

    public void OnSceneGUI()
    {
        CubicHermiteCurve curve = (CubicHermiteCurve)target;

        Handles.color = Color.white;
        curve.begin = Handles.FreeMoveHandle(curve.begin, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);
        curve.end = Handles.FreeMoveHandle(curve.end, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);

        Handles.color = Color.red;
        curve.tangentBegin = Handles.FreeMoveHandle(curve.begin + curve.tangentBegin, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap) - 
            new Vector3(curve.begin.x, curve.begin.y, 0);
        curve.tangentEnd = Handles.FreeMoveHandle(curve.end + curve.tangentEnd, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap) - 
            new Vector3(curve.end.x, curve.end.y, 0);

        Handles.color = Color.white;
        for (int i = 0; i < curve.path.Count - 1; ++i)
        {
            Handles.DrawLine(curve.path[i], curve.path[i + 1]);
        }

        curve.Refresh();
        SceneView.RepaintAll();
    }

}

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CurvePath))]
public class CCurvePathEditor : Editor
{
    public bool showPoints = true;
    public bool showTangents = true;
    public bool useTansformTool = false;

    public override void OnInspectorGUI()
    {
        CurvePath path = (CurvePath)target;

        if (GUILayout.Button("Reset"))
        {
            path.Reset();
        }

        if (GUILayout.Button("Add point"))
        {
            path.AddPoint();
        }

        showPoints = EditorGUILayout.Toggle("Show Points", showPoints);
        showTangents = EditorGUILayout.Toggle("Show Tangents", showTangents);
        useTansformTool = EditorGUILayout.Toggle("Use transform handle", useTansformTool);

        path.quality = Mathf.Max(EditorGUILayout.IntField("Path Quality", path.quality), 1);
    }

    public void OnSceneGUI()
    {
        CurvePath path = (CurvePath)target;

        Handles.SphereCap(0, path.handle, Quaternion.identity, 0.25f);

        for (int i = 0; i < path.points.Count - 1; i++)
        {
            Curve curve = path.curves[i];
            curve.quality = path.quality;

            if (showPoints)
            {
                Handles.color = Color.white;

                if (!useTansformTool)
                {
                    path.points[i] = curve.begin = Handles.FreeMoveHandle(curve.begin, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);
                    path.points[i + 1] = curve.end = Handles.FreeMoveHandle(curve.end, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);
                }
                else
                {
                    path.points[i] = curve.begin = Handles.PositionHandle(curve.begin, Quaternion.identity);
                    path.points[i + 1] = curve.end = Handles.PositionHandle(curve.end, Quaternion.identity);

                    Handles.SphereCap(0, curve.begin, Quaternion.identity, 0.25f);
                    Handles.SphereCap(0, curve.end, Quaternion.identity, 0.25f);
                }

                if (i != 0)
                {
                    path.curves[i - 1].end = path.points[i];
                }
                if (i >= path.curves.Count)
                {
                    path.curves[i + 1].begin = path.points[i + 1];
                } 
            }

            if (showTangents)
            {
                Handles.color = Color.red;

                if (!useTansformTool)
                {
                    curve.tangentBegin = Handles.FreeMoveHandle(curve.begin + curve.tangentBegin, Quaternion.identity, 0.25f, Vector3.one, Handles.DotCap) - curve.begin;
                    curve.tangentEnd = Handles.FreeMoveHandle(curve.end + curve.tangentEnd, Quaternion.identity, 0.25f, Vector3.one, Handles.DotCap) - curve.end;
                }
                else
                {
                    curve.tangentBegin = Handles.PositionHandle(curve.begin + curve.tangentBegin, Quaternion.identity) - curve.begin;
                    curve.tangentEnd = Handles.PositionHandle(curve.end + curve.tangentEnd, Quaternion.identity) - curve.end;

                    Handles.SphereCap(0, curve.begin + curve.tangentBegin, Quaternion.identity, 0.25f);
                    Handles.SphereCap(0,curve.end + curve.tangentEnd, Quaternion.identity, 0.25f);
                }

                Handles.DrawLine(curve.begin, curve.begin + curve.tangentBegin);
                Handles.DrawLine(curve.end, curve.end + curve.tangentEnd); 
            }

            if (curve.path.Count >= 2)
            {
                Handles.color = Color.white;
                for (int j = 0; j < curve.path.Count - 1; ++j)
                {
                    Handles.DrawLine(curve.path[j], curve.path[j + 1]);
                }
            }

            curve.Refresh();
        }

        path.Refresh();
        SceneView.RepaintAll();
    }

}
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CurvePath))]
public class CCurvePathEditor : Editor
{
    const float handleSize = 0.4f;

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

        path.showPoints = EditorGUILayout.Toggle("Show Points", path.showPoints);
        path.showTangents = EditorGUILayout.Toggle("Show Tangents", path.showTangents);
        path.useTansformTool = EditorGUILayout.Toggle("Use transform handle", path.useTansformTool);
        path.handleScale = EditorGUILayout.FloatField("Handle Scale", path.handleScale);
        path.quality = Mathf.Max(EditorGUILayout.IntField("Path Quality", path.quality), 1);
    }

    public void OnSceneGUI()
    {
        CurvePath path = (CurvePath)target;

        Handles.color = Color.yellow;
        Handles.SphereCap(0, path.animatedPointPosition, Quaternion.identity, handleSize * path.handleScale * 2);

        for (int i = 0; i < path.points.Count - 1; i++)
        {
            Curve curve = path.curves[i];
            curve.quality = path.quality;

            if (path.showPoints)
            {
                Handles.color = Color.white;

                if (!path.useTansformTool)
                {
                    path.points[i] = curve.begin = Handles.FreeMoveHandle(curve.begin, Quaternion.identity, handleSize * path.handleScale, Vector3.zero, Handles.DotCap);
                    path.points[i + 1] = curve.end = Handles.FreeMoveHandle(curve.end, Quaternion.identity, handleSize * path.handleScale, Vector3.zero, Handles.DotCap);
                }
                else
                {
                    path.points[i] = curve.begin = Handles.PositionHandle(curve.begin, Quaternion.identity);
                    path.points[i + 1] = curve.end = Handles.PositionHandle(curve.end, Quaternion.identity);

                    Handles.SphereCap(0, curve.begin, Quaternion.identity, handleSize * path.handleScale);
                    Handles.SphereCap(0, curve.end, Quaternion.identity, handleSize * path.handleScale);
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

            if (path.showTangents)
            {
                Handles.color = Color.red;

                if (!path.useTansformTool)
                {
                    curve.tangentBegin = Handles.FreeMoveHandle(curve.begin + curve.tangentBegin, Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap) - curve.begin;
                    curve.tangentEnd = Handles.FreeMoveHandle(curve.end + curve.tangentEnd, Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap) - curve.end;
                }
                else
                {
                    curve.tangentBegin = Handles.PositionHandle(curve.begin + curve.tangentBegin, Quaternion.identity) - curve.begin;
                    curve.tangentEnd = Handles.PositionHandle(curve.end + curve.tangentEnd, Quaternion.identity) - curve.end;

                    Handles.SphereCap(0, curve.begin + curve.tangentBegin, Quaternion.identity, handleSize * path.handleScale);
                    Handles.SphereCap(0, curve.end + curve.tangentEnd, Quaternion.identity, handleSize * path.handleScale);
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
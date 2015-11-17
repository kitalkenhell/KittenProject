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
        path.fill = EditorGUILayout.Toggle("Fill", path.fill);
        if (path.fill)
        {
            path.addCollider = EditorGUILayout.Toggle("Add Collider", path.addCollider); 
        }
        path.quality = Mathf.Max(EditorGUILayout.IntField("Path Quality", path.quality), 1);
    }

    void ProcessEvents()
    {
        CurvePath path = (CurvePath)target;

        if (Event.current.type == EventType.ValidateCommand)
        {
            if (Event.current.commandName.Contains("Delete"))
            {
                path.FreeMeshAsset();
            }
        }
    }

    public void OnSceneGUI()
    {
        CurvePath path = (CurvePath)target;

        ProcessEvents();

        for (int i = 0; i < path.points.Count - 1; i++)
        {
            Curve curve = path.curves[i];
            curve.quality = path.quality;

            Vector3 globalBegin = path.transform.TransformPoint(curve.begin);
            Vector3 globalEnd = path.transform.TransformPoint(curve.end);

            if (path.showPoints)
            {
                Handles.color = Color.white;

                if (!path.useTansformTool)
                {
                    path.points[i] = curve.begin = Handles.FreeMoveHandle(globalBegin, Quaternion.identity, handleSize * path.handleScale, Vector3.zero, Handles.DotCap);
                    path.points[i + 1] = curve.end = Handles.FreeMoveHandle(globalEnd, Quaternion.identity, handleSize * path.handleScale, Vector3.zero, Handles.DotCap);
                }
                else
                {
                    curve.begin = Handles.PositionHandle(globalBegin, Quaternion.identity);
                    curve.end = Handles.PositionHandle(globalEnd, Quaternion.identity);

                    Handles.SphereCap(0, globalBegin, Quaternion.identity, handleSize * path.handleScale);
                    Handles.SphereCap(0, globalEnd, Quaternion.identity, handleSize * path.handleScale);
                }

                path.points[i] = curve.begin = path.transform.InverseTransformPoint(curve.begin);
                path.points[i + 1] = curve.end = path.transform.InverseTransformPoint(curve.end);

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
                    curve.tangentBegin = path.transform.InverseTransformPoint(Handles.FreeMoveHandle(path.transform.TransformPoint(curve.begin + curve.tangentBegin), Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap)) - curve.begin;
                    curve.tangentEnd = path.transform.InverseTransformPoint(Handles.FreeMoveHandle(path.transform.TransformPoint(curve.end + curve.tangentEnd), Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap)) - curve.end;
                }
                else
                {
                    curve.tangentBegin = Handles.PositionHandle(curve.begin + curve.tangentBegin, Quaternion.identity) - curve.begin;
                    curve.tangentEnd = Handles.PositionHandle(curve.end + curve.tangentEnd, Quaternion.identity) - curve.end;

                    Handles.SphereCap(0, curve.begin + curve.tangentBegin, Quaternion.identity, handleSize * path.handleScale);
                    Handles.SphereCap(0, curve.end + curve.tangentEnd, Quaternion.identity, handleSize * path.handleScale);
                }

                Handles.DrawLine(path.transform.TransformPoint(curve.begin), path.transform.TransformPoint(curve.begin + curve.tangentBegin));
                Handles.DrawLine(path.transform.TransformPoint(curve.end), path.transform.TransformPoint(curve.end + curve.tangentEnd)); 
            }

            if (curve.path.Count >= 2)
            {
                Handles.color = Color.white;
                for (int j = 0; j < curve.path.Count - 1; ++j)
                {
                    Handles.DrawLine(path.transform.TransformPoint(curve.path[j]), path.transform.TransformPoint(curve.path[j + 1]));
                }
            }

            curve.Refresh();
        }

        path.Refresh();
        SceneView.RepaintAll();
    }

}
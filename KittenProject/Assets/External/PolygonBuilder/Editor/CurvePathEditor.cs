using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CurvePath))]
public class CurvePathEditor : Editor
{
    const float handleSize = 0.4f;

    public override void OnInspectorGUI()
    {
        CurvePath path = (CurvePath)target;

        if (GUILayout.Button("Reset"))
        {
            path.Reset();
        }

        if (GUILayout.Button("Ceneter Pivot"))
        {
            path.CenterPivot();
        }

        path.showPoints = EditorGUILayout.Toggle("Show Points", path.showPoints);
        path.showTangents = EditorGUILayout.Toggle("Show Tangents", path.showTangents);
        path.useTansformTool = EditorGUILayout.Toggle("Use transform handle", path.useTansformTool);
        path.handleScale = EditorGUILayout.FloatField("Handle Scale", path.handleScale);
        path.fill = EditorGUILayout.Toggle("Fill", path.fill);
        path.loop = EditorGUILayout.Toggle("Loop", path.loop);
        if (path.fill)
        {
            path.addCollider = EditorGUILayout.Toggle("Add Collider", path.addCollider); 
        }
        path.quality = Mathf.Max(EditorGUILayout.IntField("Path Quality", path.quality), 1);
    }

    void ProcessEvents()
    {
        CurvePath path = (CurvePath)target;

        if (Event.current.type == EventType.keyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:
                    path.AddPoint();
                    break;

                case KeyCode.X:
                    path.RemovePoint(Event.current.mousePosition);
                    break;
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

                    if (!path.loop || i + 1 < path.points.Count - 1)
                    {
                        path.points[i + 1] = curve.end = Handles.FreeMoveHandle(globalEnd, Quaternion.identity, handleSize * path.handleScale, Vector3.zero, Handles.DotCap);
                    }
                }
                else
                {
                    curve.begin = Handles.PositionHandle(globalBegin, Quaternion.identity);

                    if (!path.loop || i + 1 < path.points.Count - 1)
                    {
                        curve.end = Handles.PositionHandle(globalEnd, Quaternion.identity); 
                    }
                }

                path.points[i] = curve.begin = path.transform.InverseTransformPoint(curve.begin);

                if (path.loop && i + 1 == path.points.Count - 1)
                {
                    path.points[i + 1] = path.curves[0].begin; 
                }
                else
                {
                    path.points[i + 1] = curve.end = path.transform.InverseTransformPoint(curve.end); 
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

                curve.beginTangent = path.transform.InverseTransformPoint(Handles.FreeMoveHandle(path.transform.TransformPoint(curve.begin + curve.beginTangent), Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap)) - curve.begin;
                curve.endTangent = path.transform.InverseTransformPoint(Handles.FreeMoveHandle(path.transform.TransformPoint(curve.end + curve.endTangent), Quaternion.identity, handleSize * path.handleScale, Vector3.one, Handles.DotCap)) - curve.end;

                Handles.DrawLine(path.transform.TransformPoint(curve.begin), path.transform.TransformPoint(curve.begin + curve.beginTangent));
                Handles.DrawLine(path.transform.TransformPoint(curve.end), path.transform.TransformPoint(curve.end + curve.endTangent)); 
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
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PolygonBuilder))]
public class MeshBuilderEditor : Editor
{
    int virtualHandle = 0;
    bool multiselect = false;
    double lastOnSceneGUI = 0;
    double lastMouseClick = 0;
    PolygonBuilder builder;

    public override void OnInspectorGUI()
    {
        PolygonBuilder builder = (PolygonBuilder)target;

        if (GUILayout.Button("Build Quad"))
        {
            builder.BuildQuad();
        }
    }

    public void OnSceneGUI()
    {
        const float buttonSize = 0.1f;
        const float buttonPickSize = 0.5f;
        const float deselectedTimeThreshold = 0.5f;
        const float doubleClickTimeThreshold = 0.5f;

        builder = (PolygonBuilder)target;

        /*
        if (lastOnSceneGUI + deselectedTimeThreshold < EditorApplication.timeSinceStartup)
        {
            builder.selection.Clear();
        }
        lastOnSceneGUI = EditorApplication.timeSinceStartup;
        */

        multiselect = false;
        switch  (Event.current.type)
        {
            case EventType.keyDown:
                if (Event.current.keyCode == (KeyCode.LeftControl))
                {
                    multiselect = true;
                }
                break;

            case EventType.mouseDown:
                if (EditorApplication.timeSinceStartup < lastMouseClick + doubleClickTimeThreshold)
                {
                    Debug.Log("DoubleClick");
                }

                lastMouseClick = EditorApplication.timeSinceStartup;
                break;
        }

        for (int i = 0; i < builder.triangles.Length; i += 3)
        {
            Vector3 a = builder.transform.TransformPoint(builder.vertices[builder.triangles[i]]);
            Vector3 b = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 1]]);
            Vector3 c = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 2]]);

            Handles.DrawLine(a, b);
            Handles.DrawLine(b, c);
            Handles.DrawLine(c, a);
        }

        for (int i = 0; i < builder.vertices.Length; i++)
        {
            Vector4 position = builder.transform.TransformPoint(builder.vertices[i]);
            bool selected = Handles.Button(position, Quaternion.identity, buttonSize, buttonPickSize, Handles.DotCap);

            if (selected)
            {
                if (builder.selectionMode != PolygonBuilder.SelectionMode.vertex)
                {
                    builder.selection.Clear();
                }
                builder.selectionMode = PolygonBuilder.SelectionMode.vertex;

                if (multiselect)
                {
                    builder.selection.Add(i);
                }
                else
                {
                    builder.selection.Clear();
                    builder.selection.Add(i);
                }

                virtualHandle = i;
                return;
            }
        }

        if (builder.selection.Count == 0)
        {
            Tools.current = UnityEditor.Tool.Move;
            builder.transform.position = Handles.PositionHandle(builder.transform.position, Quaternion.identity);
        }
        else
        {
            Tools.current = UnityEditor.Tool.None;

            if (builder.selectionMode == PolygonBuilder.SelectionMode.vertex)
            {
                Vector4 position = builder.transform.TransformPoint(builder.vertices[virtualHandle]);
                builder.vertices[virtualHandle] = builder.transform.InverseTransformPoint(Handles.PositionHandle(position, Quaternion.identity));
            }
        }

        builder.Refresh();
    }
}
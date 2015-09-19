using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PolygonBuilder))]
public class MeshBuilderEditor : Editor
{
    const float buttonSize = 0.1f;
    const float buttonPickSize = 0.3f;
    const float doubleClickTimeThreshold = 0.25f;

    Vector3 handler = Vector3.zero;
    Vector3 handlerLastFrame = Vector3.zero;
    Vector2 mousePosition;
    bool multiselect = false;
    bool doubleClick = false;
    bool click = false;
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

        builder = (PolygonBuilder)target;

        ProcessEvents();
        //DrawTriangles();
        HandleSelection();
        HandleDoubleClick();

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
                Vector3 position = builder.transform.TransformPoint(handler);
                Vector3 displacement;

                handler = builder.transform.InverseTransformPoint(Handles.PositionHandle(position, Quaternion.identity));
                displacement = handler - handlerLastFrame;

                for (int i = 0; i < builder.selection.Count; i++)
                {
                    builder.vertices[builder.selection[i]] += displacement;
                }
            }
        }

        handlerLastFrame = handler;
        builder.Refresh();
    }

    void ProcessEvents()
    {
        bool multiselectThisFrame = false;
        click = doubleClick = false;

        mousePosition = Event.current.mousePosition;
        mousePosition.y = Screen.height - mousePosition.y;

        if (Event.current.control)
        {
            multiselectThisFrame = true;
            multiselect = true;
        }
        else if (!multiselectThisFrame)
        {
            multiselect = false;
        }
        switch (Event.current.type)
        {
            case EventType.mouseDown:
                click = true;

                if (EditorApplication.timeSinceStartup < lastMouseClick + doubleClickTimeThreshold)
                {
                    doubleClick = true;
                    lastMouseClick = 0;
                }
                else
                { 
                    lastMouseClick = EditorApplication.timeSinceStartup;
                }
                break;
        }
    }

    void DrawTriangles()
    {
        for (int i = 0; i < builder.triangles.Length; i += 3)
        {
            Vector3 a = builder.transform.TransformPoint(builder.vertices[builder.triangles[i]]);
            Vector3 b = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 1]]);
            Vector3 c = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 2]]);

            Handles.DrawLine(a, b);
            Handles.DrawLine(b, c);
            Handles.DrawLine(c, a);
        }
    }

    void HandleSelection()
    {
        bool clickedButton = false;

        for (int i = 0; i < builder.vertices.Length; i++)
        {
            Vector4 position = builder.transform.TransformPoint(builder.vertices[i]);

            if (builder.selectionMode == PolygonBuilder.SelectionMode.vertex && builder.selection.Contains(i))
            {
                Handles.color = Color.magenta;
            }
            else
            {
                Handles.color = Color.white;
            }

            bool selected = Handles.Button(position, Quaternion.identity, buttonSize, buttonPickSize, Handles.DotCap);

            if (selected)
            {
                clickedButton = true;

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

                handler = handlerLastFrame = builder.vertices[i];
                return;
            }
        }

        if (doubleClick && !clickedButton)
        {
            builder.selection.Clear();
        }
    }

    void HandleDoubleClick()
    {
        float closest = Mathf.Infinity;
        int closestIndex = 0;

        if (!doubleClick)
        {
            //return;
        }

        for (int i = 0; i < builder.triangles.Length - 1; ++i)
        {
            Vector3 begin = Camera.current.WorldToScreenPoint(builder.transform.TransformPoint(builder.vertices[builder.triangles[i]]));
            Vector3 end = Camera.current.WorldToScreenPoint(builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 1]]));

            float distance = HandleUtility.DistancePointToLineSegment(mousePosition, begin, end);

            if (distance < closest)
            {
                closest = distance;
                closestIndex = i;
            }
        }

        Handles.color = Color.white;
        Handles.DrawLine(builder.transform.TransformPoint(builder.vertices[builder.triangles[closestIndex]]), builder.transform.TransformPoint(builder.vertices[builder.triangles[closestIndex + 1]]));
    }

    bool isOuterEdge(int vertexA, int vertexB)
    {
        bool first = true;

        for (int i = 0; i < builder.triangles.Length; i += 3)
        {
            if ( ((vertexA == builder.triangles[i] && vertexB == builder.triangles[i + 1]) || (vertexB == builder.triangles[i] && vertexA == builder.triangles[i + 1])) ||
                 ((vertexA == builder.triangles[i + 1] && vertexB == builder.triangles[i + 2]) || (vertexB == builder.triangles[i + 1] && vertexA == builder.triangles[i + 2])) ||
                 ((vertexA == builder.triangles[i + 2] && vertexB == builder.triangles[i]) || (vertexB == builder.triangles[i + 2] && vertexA == builder.triangles[i])) )
            {
                if (!first)
                {
                    return false;
                }
                first = false;
            }
        }

        return true;
    }
}
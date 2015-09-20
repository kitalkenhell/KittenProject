using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(PolygonBuilder))]
public class MeshBuilderEditor : Editor
{
    const float buttonSize = 0.1f;
    const float buttonPickSize = 0.3f;
    const float doubleClickTimeThreshold = 0.25f;
    const float pickLineDistanceThreshold = 12.0f;
    const float pickLineDiscSize = 0.2f;

    PolygonBuilder builder;
    Vector3 handler = Vector3.zero;
    Vector3 handlerLastFrame = Vector3.zero;
    
    bool multiselect = false;
    bool doubleClick = false;
    bool delete = false;
    double lastMouseClick = 0;
    Vector2 mousePosition;
    
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

        HandleDelete();

        handlerLastFrame = handler;
        builder.Refresh();
    }

    void ProcessEvents()
    {
        bool multiselectThisFrame = false;

        mousePosition = Event.current.mousePosition;
        mousePosition.y = Screen.height - mousePosition.y;
        delete = false;
        
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

            case EventType.keyDown:
                if (Event.current.keyCode == KeyCode.X)
                {
                    delete = true;
                }
                break;
        }
    }

    void DrawTriangles()
    {
        for (int i = 0; i < builder.triangles.Count; i += 3)
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
        for (int i = 0; i < builder.vertices.Count; i++)
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
    }

    void HandleDoubleClick()
    {
        float closest = Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < builder.triangles.Count - 1; ++i)
        {
            Vector3 begin = builder.transform.TransformPoint(builder.vertices[builder.triangles[i]]);
            Vector3 end = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 1]]);

            float distance = HandleUtility.DistanceToLine(begin, end);

            if (distance < closest)
            {
                closest = distance;
                closestIndex = i;
            }
        }

        if (closest < pickLineDistanceThreshold)
        {
            int beginIdexA = builder.triangles[closestIndex];
            int beginIdexB = builder.triangles[closestIndex + 1];

            Vector3 vertex = HandleUtility.ClosestPointToPolyLine(
                new Vector3[] { builder.transform.TransformPoint(builder.vertices[beginIdexA]), builder.transform.TransformPoint(builder.vertices[beginIdexB]) });

            if (doubleClick)
            {
                builder.vertices.Add(builder.transform.InverseTransformPoint(vertex));
                builder.colors.Add((builder.colors[beginIdexA] + builder.colors[beginIdexB]) / 2.0f);
                builder.triangles.AddRange(new int[] { beginIdexA, builder.vertices.Count - 1, beginIdexB });

                builder.selection.Clear();
                builder.selection.Add(builder.vertices.Count - 1);
                handler = handlerLastFrame = builder.vertices[builder.vertices.Count - 1];
            }
        }

        doubleClick = false;
    }

    void HandleDelete()
    {
        if (!delete)
        {
            return;
        }

        List<int> toRemoveTriangles = new List<int>();
        List<int> toRemoveVertices = new List<int>();

        foreach (var vertex in builder.selection)
	    {
            for (int i = 0; i < builder.triangles.Count; i += 3)
            {
                if (builder.triangles[i] == vertex || builder.triangles[i + 1] == vertex || builder.triangles[i + 2] == vertex)
                {
                    toRemoveTriangles.Add(i);
                    toRemoveTriangles.Add(i + 1);
                    toRemoveTriangles.Add(i + 2);
                }
            }

            toRemoveVertices.Add(vertex);
        }

        toRemoveVertices.Sort();

        for (int i = toRemoveTriangles.Count - 1; i >= 0; i--)
        {
            builder.triangles.RemoveAt(toRemoveTriangles[i]);
        }

        for (int i = toRemoveVertices.Count - 1; i >= 0; i--)
        {
            builder.vertices.RemoveAt(toRemoveVertices[i]);
            builder.colors.RemoveAt(toRemoveVertices[i]);

            for (int j = 0; j < builder.triangles.Count; ++j)
            {
                if (builder.triangles[j] > toRemoveVertices[i])
                {
                    --builder.triangles[j];
                }
            }
        }

        builder.selection.Clear();
    }

    bool isOuterEdge(int vertexA, int vertexB)
    {
        bool first = true;

        for (int i = 0; i < builder.triangles.Count; i += 3)
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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(PolygonBuilder))]
public class MeshBuilderEditor : Editor
{
    const float vertexButtonSize = 0.075f;
    const float vertexButtonPickSize = 0.2f;
    const float doubleClickTimeThreshold = 0.25f;
    const float pickLineDistanceThreshold = 12.0f;

    PolygonBuilder builder;
    Vector3 handler = Vector3.zero;
    Vector3 handlerLastFrame = Vector3.zero;
    
    bool multiselect = false;
    bool addVertexAndTriangle = false;
    bool addVertex = false;
    bool createTriangle = false;
    bool delete = false;
    bool align = false;
    bool extrude = false;
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
        Selection();
        Translation();
        AddVertex();
        Align();
        Extrude();
        CreateTriangle();
        SetProperNormals();
        Delete();
        
        builder.Refresh();

        SceneView.RepaintAll();
    }

    void ProcessEvents()
    {
        bool multiselectThisFrame = false;

        mousePosition = Event.current.mousePosition;
        mousePosition.y = Screen.height - mousePosition.y;

        delete = false;
        createTriangle = false;
        align = false;
        extrude = false;
        addVertex = false;

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
                    addVertexAndTriangle = true;
                    lastMouseClick = 0;
                }
                else
                {
                    lastMouseClick = EditorApplication.timeSinceStartup;
                }
                break;

            case EventType.keyDown:
                switch (Event.current.keyCode)
                {
                    case KeyCode.X:
                        delete = true;
                        break;

                    case KeyCode.T:
                        createTriangle = true;
                        break;

                    case KeyCode.A:
                        align = true;
                        break;

                    case KeyCode.E:
                        extrude = true;
                        break;

                    case KeyCode.V:
                        addVertex = true;
                        break;
                }
                break;
        }
    }

    void Translation()
    {
        if (builder.selection.Count == 0)
        {
            Tools.current = UnityEditor.Tool.Move;
            Handles.color = Color.magenta;
            builder.transform.position = Handles.PositionHandle(builder.transform.position, Quaternion.identity);
        }
        else
        {
            Tools.current = UnityEditor.Tool.None;

            if (builder.selectionMode == PolygonBuilder.SelectionMode.vertex)
            {
                Vector3 position = builder.transform.TransformPoint(handler);
                Vector3 displacement;

                Handles.color = Color.magenta;
                handler = builder.transform.InverseTransformPoint(Handles.PositionHandle(position, Quaternion.identity));
                displacement = handler - handlerLastFrame;

                for (int i = 0; i < builder.selection.Count; i++)
                {
                    builder.vertices[builder.selection[i]] += displacement;
                }
            }
        }

        handlerLastFrame = handler;
    }

    bool DetectAxisToAlign()
    {
        Vector2 distance = Vector2.zero;
        

        for (int i = 0; i < builder.selection.Count - 1; ++i)
        {
            distance.x += Mathf.Abs(builder.vertices[builder.selection[i]].x - builder.vertices[builder.selection[i + 1]].x);
            distance.y += Mathf.Abs(builder.vertices[builder.selection[i]].y - builder.vertices[builder.selection[i + 1]].y);
        }

        return distance.x < distance.y;
    }

    void Align()
    {
        if (!align || builder.selection.Count <= 1)
        {
            return;
        }

        Vector3 mean = Vector3.zero;
        foreach (var index in builder.selection)
        {
            mean += builder.vertices[index];
        }
        mean /= builder.selection.Count;

        if (DetectAxisToAlign())
        {
            for (int i = 0; i < builder.selection.Count; ++i)
            {
                builder.vertices[builder.selection[i]] = new Vector3(mean.x, builder.vertices[builder.selection[i]].y, builder.vertices[builder.selection[i]].z);
                handler = handlerLastFrame = builder.vertices[builder.selection[i]];
            }
        }
        else
        {
            for (int i = 0; i < builder.selection.Count; ++i)
            {
                builder.vertices[builder.selection[i]] = new Vector3(builder.vertices[builder.selection[i]].x, mean.y, builder.vertices[builder.selection[i]].z);
                handler = handlerLastFrame = builder.vertices[builder.selection[i]];
            }
        }

    }

    void Extrude()
    {
        if (!extrude || builder.selection.Count <= 1)
        {
            return;
        }

        Utils.Tuple<Vector3, int>[] newVertices = new Utils.Tuple<Vector3, int>[builder.selection.Count];
        Vector3 displacement; 

        for (int i = 0; i < builder.selection.Count; ++i)
		{
            newVertices[i].First = builder.vertices[builder.selection[i]];
            newVertices[i].Second = builder.selection[i]; 
		}

        if (DetectAxisToAlign())
        {
            displacement = Vector3.right;
            Array.Sort(newVertices, (vert1, vert2) => vert1.First.x.CompareTo(vert2.First.x)); 
        }
        else
        {
            displacement = Vector3.up;
            Array.Sort(newVertices, (vert1, vert2) => vert1.First.y.CompareTo(vert2.First.y));
        }

        builder.selection.Clear();
        for (int i = 0; i < newVertices.Length; ++i)
        {
            builder.vertices.Add(newVertices[i].First + displacement);
            builder.colors.Add(builder.colors[newVertices[i].Second]);
            builder.selection.Add(builder.vertices.Count - 1);
        }
        handler = handlerLastFrame = builder.vertices[builder.vertices.Count - 1];

        for (int i = 0; i < newVertices.Length - 1; ++i)
        {
            int a = newVertices[i].Second;
            int b = newVertices[i + 1].Second;
            int c = builder.vertices.Count - newVertices.Length + i;

            builder.triangles.Add(a);
            builder.triangles.Add(b);
            builder.triangles.Add(c);

            a = builder.vertices.Count - newVertices.Length + i;
            b = builder.vertices.Count - newVertices.Length + i + 1;
            c = newVertices[i + 1].Second;

            builder.triangles.Add(a);
            builder.triangles.Add(b);
            builder.triangles.Add(c);
        }
    }

    void Selection()
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

            bool selected = Handles.Button(position, Quaternion.identity, vertexButtonSize, vertexButtonPickSize, Handles.DotCap);

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

                lastMouseClick = 0;
                handler = handlerLastFrame = builder.vertices[i];
                return;
            }
        }
    }

    void AddVertex()
    {
        float closest = Mathf.Infinity;
        int closestIndexBegin = 0;
        int closestIndexEnd = 0;
        int endIndex;

        for (int i = 0; i < builder.triangles.Count; ++i)
        {
            Vector3 begin = builder.transform.TransformPoint(builder.vertices[builder.triangles[i]]);
            Vector3 end;
            endIndex = i + 1;

            if ((i + 1) % 3 == 0)
            {
                end = builder.transform.TransformPoint(builder.vertices[builder.triangles[i - 2]]);
                endIndex = i - 2;
            }
            else
            {
                end = builder.transform.TransformPoint(builder.vertices[builder.triangles[i + 1]]);
            }

            float distance = HandleUtility.DistanceToLine(begin, end);

            if (distance < closest)
            {
                closest = distance;
                closestIndexBegin = i;
                closestIndexEnd = endIndex;
            }
        }

        if (closest < pickLineDistanceThreshold)
        {
            int edgeBeginIndex = builder.triangles[closestIndexBegin];
            int edgeEndIndex = builder.triangles[closestIndexEnd];

            Vector3 vertex = HandleUtility.ClosestPointToPolyLine(
                new Vector3[] { builder.transform.TransformPoint(builder.vertices[edgeBeginIndex]), builder.transform.TransformPoint(builder.vertices[edgeEndIndex]) });

            Handles.color = Color.white;
            Handles.DrawLine(builder.transform.TransformPoint(builder.vertices[edgeBeginIndex]), builder.transform.TransformPoint(builder.vertices[edgeEndIndex]));
            Handles.DrawSolidDisc(vertex, Vector3.forward, vertexButtonSize);

            if (addVertexAndTriangle)
            {
                builder.vertices.Add(builder.transform.InverseTransformPoint(vertex));
                builder.colors.Add((builder.colors[edgeBeginIndex] + builder.colors[edgeEndIndex]) / 2.0f);
                builder.triangles.AddRange(new int[] { edgeBeginIndex, builder.vertices.Count - 1, edgeEndIndex });

                builder.selection.Clear();
                builder.selection.Add(builder.vertices.Count - 1);
                handler = handlerLastFrame = builder.vertices[builder.vertices.Count - 1];
            }
            else if (addVertex)
            {
                List<int> newTriangles = new List<int>();
                List<int> toRemoveTriangles = new List<int>();

                builder.vertices.Add(builder.transform.InverseTransformPoint(vertex));
                builder.colors.Add((builder.colors[edgeBeginIndex] + builder.colors[edgeEndIndex]) / 2.0f);

                builder.selection.Clear();

                for (int i = 0; i < builder.triangles.Count; i += 3)
                {
                    
                    int count = 0;
                    if (builder.triangles[i] == edgeBeginIndex || builder.triangles[i] == edgeEndIndex) ++count;
                    if (builder.triangles[i + 1] == edgeBeginIndex || builder.triangles[i + 1] == edgeEndIndex) ++count;
                    if (builder.triangles[i + 2] == edgeBeginIndex || builder.triangles[i + 2] == edgeEndIndex) ++count;

                    if (count >= 2)
                    {
                        int free = 0;
                        
                        if (builder.triangles[i+1] != edgeBeginIndex && builder.triangles[i+1] != edgeEndIndex) free = 1;
                        else if (builder.triangles[i+2] != edgeBeginIndex && builder.triangles[i+2] != edgeEndIndex) free = 2;

                        Debug.Log("" + builder.triangles[(i + free)] + " " + builder.triangles[edgeBeginIndex] + " " + builder.triangles[(builder.vertices.Count - 1)]);

                        newTriangles.Add(builder.triangles[i + free]);
                        newTriangles.Add(edgeBeginIndex);
                        newTriangles.Add(builder.vertices.Count - 1);

                        newTriangles.Add(builder.triangles[i + free]);
                        newTriangles.Add(edgeEndIndex);
                        newTriangles.Add(builder.vertices.Count - 1);

                        toRemoveTriangles.Add(i);
                        toRemoveTriangles.Add(i + 1);
                        toRemoveTriangles.Add(i + 2);

                        builder.selection.Add(edgeEndIndex);
                    }
                }

                toRemoveTriangles.Sort();
                toRemoveTriangles = toRemoveTriangles.Distinct().ToList();

                for (int i = toRemoveTriangles.Count - 1; i >= 0; i--)
                {
                    builder.triangles.RemoveAt(toRemoveTriangles[i]);
                }

                builder.triangles.AddRange(newTriangles);
            }
        }

        addVertexAndTriangle = false;
    }

    void Delete()
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

        toRemoveTriangles.Sort();
        toRemoveVertices.Sort();

        toRemoveTriangles = toRemoveTriangles.Distinct().ToList();
        toRemoveVertices = toRemoveVertices.Distinct().ToList();

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

    void CreateTriangle()
    {
        if (!createTriangle || builder.selection.Count != 3)
        {
            return;
        }

        int a = builder.selection[0];
        int b = builder.selection[1];
        int c = builder.selection[2];

        if (HasTriangle(a,b,c))
        {
            return;
        }

        builder.triangles.Add(a);
        builder.triangles.Add(b);
        builder.triangles.Add(c);
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

    bool HasTriangle(int a, int b, int c)
    {
        for (int i = 0; i < builder.triangles.Count; i += 3)
        {
            if ((builder.triangles[i] == a || builder.triangles[i + 1] == a || builder.triangles[i + 2] == a) &&
                (builder.triangles[i] == b || builder.triangles[i + 1] == b || builder.triangles[i + 2] == b) &&
                (builder.triangles[i] == c || builder.triangles[i + 1] == c || builder.triangles[i + 2] == c))
            {
                return true;
            }
        }
        return false;
    }

    void SetProperNormals()
    {
        for (int i = 0; i < builder.triangles.Count - 2; i += 3)
        {

            if (Vector3.Cross(builder.vertices[builder.triangles[i]] - builder.vertices[builder.triangles[i + 1]], builder.vertices[builder.triangles[i + 2]] - builder.vertices[builder.triangles[i]]).z < 0)
            {
                int tmp = builder.triangles[i];
                builder.triangles[i] = builder.triangles[i + 2];
                builder.triangles[i + 2] = tmp;
            }
        }
    }
}
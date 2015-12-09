using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(PolygonBuilder))]
public class MeshBuilderEditor : Editor
{
    const float vertexButtonSize = 0.1f;
    const float vertexButtonPickSize = 0.2f;
    const float gradientHandleSize = 0.075f;
    const float newVertexCircleSize = 0.025f;
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
    bool deselect = false;
    double lastMouseClickTime = 0;
    Vector2 mousePosition;
    
    public override void OnInspectorGUI()
    {
        PolygonBuilder builder = (PolygonBuilder)target;
        MeshFilter meshFilter = builder.GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            if (GUILayout.Button("Initialize"))
            {
                builder.BuildSquare(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(builder)).Replace("PolygonBuilder.cs", ""));
            }
        }
        else if (GUILayout.Button("Build Collider"))
        {
            builder.BuildCollider();
        }

        builder.uvScale = EditorGUILayout.FloatField("UV Scale", builder.uvScale);
        builder.uvOffset = -EditorGUILayout.Vector2Field("UV Offset", -builder.uvOffset);
        builder.buildCollider = EditorGUILayout.Toggle("Build Collider", builder.buildCollider);
        builder.autoTriangulation = EditorGUILayout.Toggle("Auto Triangulation", builder.autoTriangulation);
        builder.lockPolygon = EditorGUILayout.Toggle("Allow Editing", builder.lockPolygon);

        //builder.reflectiveGradientShader = EditorGUILayout.ObjectField(builder.reflectiveGradientShader, typeof(Shader), false) as Shader;
        //builder.linearGradientShader = EditorGUILayout.ObjectField(builder.linearGradientShader, typeof(Shader), false) as Shader;
        //builder.radialGradientShader = EditorGUILayout.ObjectField(builder.radialGradientShader, typeof(Shader), false) as Shader;
        //builder.conicalGradientShader = EditorGUILayout.ObjectField(builder.conicalGradientShader, typeof(Shader), false) as Shader;
        //builder.vertexColorShader = EditorGUILayout.ObjectField(builder.vertexColorShader, typeof(Shader), false) as Shader;

        builder.renderMode = (PolygonBuilder.RenderMode)EditorGUILayout.EnumPopup("Render Mode: ", builder.renderMode);

        if (builder.renderMode == PolygonBuilder.RenderMode.customMaterial)
	    {
		    builder.customMaterial = EditorGUILayout.ObjectField("Custom Material", builder.customMaterial, typeof(Material), false) as Material; 
	    }
    }

    public void OnSceneGUI()
    {
        builder = (PolygonBuilder) target;
        MeshFilter meshFilter = builder.GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            if (builder.lockPolygon)
            {
                builder.transform.position = Handles.PositionHandle(builder.transform.position, Quaternion.identity);
            }
            else
            {
                ProcessEvents();
                Selection();
                Translation();
                AddVertex();
                Align();
                Extrude();
                CreateTriangle();
                Delete();

                builder.Refresh();
            }

            EditorUtility.SetDirty(builder);
            SceneView.RepaintAll();
        }
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
        deselect = false;

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
                if (EditorApplication.timeSinceStartup < lastMouseClickTime + doubleClickTimeThreshold)
                {
                    addVertexAndTriangle = true;
                    lastMouseClickTime = 0;
                }
                else
                {
                    lastMouseClickTime = EditorApplication.timeSinceStartup;
                }
                break;

            case EventType.mouseUp:
                const int rightMouseButton = 1;

                if (Event.current.button == rightMouseButton)
                {
                    deselect = true;
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

                    case KeyCode.Escape:
                        deselect = true;
                        break;
                }
                break;

            case EventType.ValidateCommand:
                if (Event.current.commandName.Contains("Delete"))
                {
                    builder.FreeAssets();
                }
                if (Event.current.commandName.Contains("Copy"))
                {
                    Event.current.Use();
                }
                if (Event.current.commandName.Contains("Duplicate"))
                {
                    Event.current.Use();
                }
                break;
        }
    }

    void Translation()
    {
        if (builder.selection.Count == 0)
        {
            Vector3 middle = Vector3.zero;
            Vector3 displacement; 

            foreach (var vertex in builder.vertices)
            {
                middle += vertex;
            }

            middle /= builder.vertices.Count;
            displacement = middle;

            middle = builder.transform.InverseTransformPoint(Handles.PositionHandle(builder.transform.TransformPoint(middle), Quaternion.identity));
            displacement = middle - displacement;

            builder.transform.position += displacement;
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

        EditorUtils.Tuple<Vector3, int>[] newVertices = new EditorUtils.Tuple<Vector3, int>[builder.selection.Count];
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

                lastMouseClickTime = 0;
                handler = handlerLastFrame = builder.vertices[i];
                return;
            }
        }

        if (builder.renderMode == PolygonBuilder.RenderMode.conicalGradient ||
            builder.renderMode == PolygonBuilder.RenderMode.linearGradient ||
            builder.renderMode == PolygonBuilder.RenderMode.radialGradient ||
            builder.renderMode == PolygonBuilder.RenderMode.reflectiveGradient)
        {
            Handles.color = Color.magenta;
            for (int i = 0; i < builder.gradientPoints.Count; ++i)
            {
                Vector4 position = builder.transform.TransformPoint(builder.gradientPoints[i].position);
                position = Handles.FreeMoveHandle(position, Quaternion.identity, gradientHandleSize, Vector3.one, Handles.DotCap);
                builder.gradientPoints[i].position = builder.transform.InverseTransformPoint(position);
            }
        }

        if (deselect)
        {
            builder.selection.Clear();
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
            Handles.DrawSolidDisc(vertex, Vector3.forward, newVertexCircleSize);

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

        if (builder.HasTriangle(a,b,c))
        {
            return;
        }

        builder.triangles.Add(a);
        builder.triangles.Add(b);
        builder.triangles.Add(c);
    }
}
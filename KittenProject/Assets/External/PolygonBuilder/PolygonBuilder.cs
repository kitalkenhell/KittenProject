#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[ExecuteInEditMode]
public class PolygonBuilder : MonoBehaviour
{
    public enum SelectionMode
    {
        vertex,
        face
    };

    public enum RenderMode
    {
        vertexColor,
        linearGradient,
        radialGradient,
        conicalGradient,
        reflectiveGradient,
        customMaterial
    }

    [System.Serializable]
    public class GradientVertex
    {
        public Vector3 position;
        public Color color;

        public GradientVertex(Vector3 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public GradientVertex()
        {
            position = Vector3.zero;
            color = Color.black;
        }
    }

    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Color> colors = new List<Color>();
    public List<int> triangles = new List<int>();
    public int[] autoTriangles;

    public SelectionMode selectionMode;
    public List<int> selection = new List<int>();

    public float uvScale = 1;
    public Vector3 uvOffset = Vector3.zero;
    public float conicalGradientCutoff = 0.8f;
    public float conicalGradientMaxAngle = 0.1f;
    public float conicalGradientMaxRange = 1;

    public RenderMode renderMode = RenderMode.vertexColor;
    public List<GradientVertex> gradientPoints = new List<GradientVertex>();

    public Shader reflectiveGradientShader;
    public Shader linearGradientShader;
    public Shader radialGradientShader;
    public Shader conicalGradientShader;
    public Shader vertexColorShader;
    public Material uniqueMaterial;
    public Material customMaterial;

    public bool buildCollider = false;
    public bool autoTriangulation = false;
    public bool lockPolygon = false;

    public string uniqueMaterialPath = "";
    public string uniqueMeshPath = "";

    Vector3[] autoVertices;
    List<int> autoIndices;
    List<Vector2> outlinePath;

    public void BuildSquare(string assetsPath)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if (meshFilter == null)
        {
            gameObject.AddComponent<MeshFilter>();
            meshFilter = GetComponent<MeshFilter>();
        }

        if (renderer == null)
        {
            gameObject.AddComponent<MeshRenderer>();
            renderer = GetComponent<MeshRenderer>();
        }

        if (meshFilter.sharedMesh == null)
        {
            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh = meshFilter.sharedMesh;
            uniqueMeshPath = AssetDatabase.GenerateUniqueAssetPath(assetsPath + "Meshes/Polygon.asset");
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, uniqueMeshPath);
        }

        if (renderMode != RenderMode.customMaterial && (renderer.sharedMaterial == null || string.IsNullOrEmpty(uniqueMaterialPath)))
        {
            uniqueMaterial = new Material(vertexColorShader);
            customMaterial = uniqueMaterial;
            renderer.sharedMaterial = uniqueMaterial;
            uniqueMaterialPath = AssetDatabase.GenerateUniqueAssetPath(assetsPath + "Materials/Material.mat");
            AssetDatabase.CreateAsset(renderer.sharedMaterial, uniqueMaterialPath);
        }

        meshFilter.sharedMesh.Clear();

        triangles.Clear();
        colors.Clear();
        vertices.Clear();

        colors.Add(new Color(0, 0, 0, 1));
        colors.Add(new Color(0, 0, 0, 1));
        colors.Add(new Color(0, 0, 0, 1));
        colors.Add(new Color(0, 0, 0, 1));

        vertices.Add(new Vector3(-0.5f, -0.5f, 0));
        vertices.Add(new Vector3(0.5f, -0.5f, 0));
        vertices.Add(new Vector3(0.5f, 0.5f, 0));
        vertices.Add(new Vector3(-0.5f, 0.5f, 0));

        triangles.AddRange(new int[] { 3, 1, 0, 3, 2, 1 });

        meshFilter.sharedMesh.vertices = vertices.ToArray();
        meshFilter.sharedMesh.colors = colors.ToArray();
        meshFilter.sharedMesh.triangles = triangles.ToArray();

        gradientPoints.Clear();
        gradientPoints.Add(new GradientVertex(Vector3.zero, Color.white));
        gradientPoints.Add(new GradientVertex(Vector3.up, Color.red));
        renderMode = RenderMode.vertexColor;

        conicalGradientCutoff = 0.8f;
        conicalGradientMaxAngle = 0.1f;
        conicalGradientMaxRange = 1;

        selection.Clear();
    }

    public void BuildOutlinePath()
    {
        List<Vector2> edges = new List<Vector2>();

        autoIndices = new List<int>();
        outlinePath = new List<Vector2>();

        for (int i = 0; i < triangles.Count; i += 3)
        {
            if (isOuterEdge(triangles[i], triangles[i + 1]))
            {
                edges.Add(new Vector2(triangles[i], triangles[i + 1]));
            }

            if (isOuterEdge(triangles[i + 1], triangles[i + 2]))
            {
                edges.Add(new Vector2(triangles[i + 1], triangles[i + 2]));
            }

            if (isOuterEdge(triangles[i + 2], triangles[i]))
            {
                edges.Add(new Vector2(triangles[i + 2], triangles[i]));
            }
        }

        outlinePath.Add(vertices[(int)edges[0].x]);
        outlinePath.Add(vertices[(int)edges[0].y]);

        autoIndices.Add((int)edges[0].x);
        autoIndices.Add((int)edges[0].y);

        edges.RemoveAt(0);

        while (edges.Count != 1)
        {

            for (int i = 0; i < edges.Count; ++i)
            {
                if (vertices[(int)edges[i].x].XY() == outlinePath[outlinePath.Count - 1])
                {
                    outlinePath.Add(vertices[(int)edges[i].y]);
                    edges.RemoveAt(i);
                    autoIndices.Add((int)edges[i].x);
                    break;
                }
                else if (vertices[(int)edges[i].y].XY() == outlinePath[outlinePath.Count - 1])
                {
                    outlinePath.Add(vertices[(int)edges[i].x]);
                    edges.RemoveAt(i);
                    autoIndices.Add((int)edges[i].y);
                    break;
                }
            }
        }

    }

    public void BuildCollider()
    {
        PolygonCollider2D collider2d = GetComponent<PolygonCollider2D>();

        if (collider2d == null)
        {
            collider2d = gameObject.AddComponent<PolygonCollider2D>();
        }

        collider2d.SetPath(0, outlinePath.ToArray());
    }

    public void OnDrawGizmos()
    {
        if (GetComponent<MeshFilter>() == null || GetComponent<MeshFilter>().sharedMesh == null)
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }

    public void Refresh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Mesh polygon;

        if (meshFilter == null || renderer.sharedMaterial == null)
        {
            return;
        }

        polygon = meshFilter.sharedMesh;

        polygon.Clear();

        if (renderMode == RenderMode.vertexColor)
        {
            renderer.sharedMaterial = uniqueMaterial;
            renderer.sharedMaterial.shader = vertexColorShader;
        }
        else if (renderMode == RenderMode.customMaterial)
        {
            renderer.sharedMaterial = customMaterial;
        }
        else if (renderMode == RenderMode.radialGradient)
        {
            const int center = 0;
            const int edge = 1;

            renderer.sharedMaterial = uniqueMaterial;
            renderer.sharedMaterial.shader = radialGradientShader;

            if (gradientPoints.Count > edge)
            {
                renderer.sharedMaterial.SetVector("center", gradientPoints[center].position);
                renderer.sharedMaterial.SetFloat("radius", Vector3.Distance(gradientPoints[center].position, gradientPoints[edge].position));
                renderer.sharedMaterial.SetColor("colorCenter", gradientPoints[center].color);
                renderer.sharedMaterial.SetColor("colorEdge", gradientPoints[edge].color);
            }
        }
        else if (renderMode == RenderMode.conicalGradient)
        {
            const int center = 0;
            const int edge = 1;

            renderer.sharedMaterial = uniqueMaterial;
            renderer.sharedMaterial.shader = conicalGradientShader;

            if (gradientPoints.Count > edge)
            {
                renderer.sharedMaterial.SetVector("center", gradientPoints[center].position);
                renderer.sharedMaterial.SetVector("direction", (gradientPoints[edge].position - gradientPoints[center].position).normalized);
                renderer.sharedMaterial.SetFloat("radius", Vector3.Distance(gradientPoints[center].position, gradientPoints[edge].position));
                renderer.sharedMaterial.SetFloat("cutoff", conicalGradientCutoff);
                renderer.sharedMaterial.SetFloat("maxRange", conicalGradientMaxRange);
                renderer.sharedMaterial.SetFloat("maxAngle", conicalGradientMaxAngle);
                renderer.sharedMaterial.SetColor("colorCenter", gradientPoints[center].color);
                renderer.sharedMaterial.SetColor("colorEdge", gradientPoints[edge].color);
            }
        }
        else if (renderMode == RenderMode.linearGradient || renderMode == RenderMode.reflectiveGradient)
        {
            const int begin = 0;
            const int end = 1;

            renderer.sharedMaterial = uniqueMaterial;
            renderer.sharedMaterial.shader = renderMode == RenderMode.linearGradient ? linearGradientShader : reflectiveGradientShader;

            if (gradientPoints.Count > end)
            {
                Vector3 direction = gradientPoints[end].position - gradientPoints[begin].position;

                renderer.sharedMaterial.SetVector("begin", gradientPoints[begin].position);
                renderer.sharedMaterial.SetVector("direction", new Vector2(-direction.y, direction.x).normalized);
                renderer.sharedMaterial.SetFloat("radius", Vector3.Distance(gradientPoints[begin].position, gradientPoints[end].position));
                renderer.sharedMaterial.SetColor("colorBegin", gradientPoints[begin].color);
                renderer.sharedMaterial.SetColor("colorEnd", gradientPoints[end].color);
            }
        }

        SetProperNormals();
        
        polygon.colors = null;

        if (autoTriangulation || buildCollider)
        {
            BuildOutlinePath();
        }

        if (autoTriangulation)
        {
            List<Color> autoColors = new List<Color>();

            autoVertices = new Vector3[outlinePath.Count];

            for (int i = 0; i < autoVertices.Length; ++i)
            {
                autoVertices[i] = new Vector3(outlinePath[i].x, outlinePath[i].y, 0);
            }

            polygon.vertices = autoVertices;
            autoTriangles = new Triangulator(outlinePath.ToArray()).Triangulate();
            polygon.triangles = autoTriangles;

            uvs.Clear();
            uvs.Capacity = autoVertices.Length;
            autoColors.Capacity = autoIndices.Count;

            for (int i = 0; i < autoIndices.Count; ++i)
            {
                autoColors.Add(colors[autoIndices[i]]);
            }
            polygon.colors = autoColors.ToArray();

            for (int i = 0; i < autoVertices.Length; ++i)
            {
                uvs.Add(autoVertices[i] * uvScale + uvOffset);
            }
            polygon.uv = uvs.ToArray();
        }
        else
        {
            polygon.vertices = vertices.ToArray();
            polygon.triangles = triangles.ToArray();
            polygon.colors = colors.ToArray();

            uvs.Clear();
            uvs.Capacity = vertices.Count;
            for (int i = 0; i < vertices.Count; ++i)
            {
                uvs.Add(vertices[i] * uvScale + uvOffset);
            }
            polygon.uv = uvs.ToArray();
        }

        if (buildCollider)
        {
            BuildCollider();
        }
    }

    public bool isOuterEdge(int vertexA, int vertexB)
    {
        bool first = true;

        for (int i = 0; i < triangles.Count; i += 3)
        {
            if (((vertexA == triangles[i] && vertexB == triangles[i + 1]) || (vertexB == triangles[i] && vertexA == triangles[i + 1])) ||
                 ((vertexA == triangles[i + 1] && vertexB == triangles[i + 2]) || (vertexB == triangles[i + 1] && vertexA == triangles[i + 2])) ||
                 ((vertexA == triangles[i + 2] && vertexB == triangles[i]) || (vertexB == triangles[i + 2] && vertexA == triangles[i])))
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

    public bool HasTriangle(int a, int b, int c)
    {
        for (int i = 0; i < triangles.Count; i += 3)
        {
            if ((triangles[i] == a || triangles[i + 1] == a || triangles[i + 2] == a) &&
                (triangles[i] == b || triangles[i + 1] == b || triangles[i + 2] == b) &&
                (triangles[i] == c || triangles[i + 1] == c || triangles[i + 2] == c))
            {
                return true;
            }
        }
        return false;
    }

    public void SetProperNormals()
    {
        for (int i = 0; i < triangles.Count - 2; i += 3)
        {

            if (Vector3.Cross(vertices[triangles[i]] - vertices[triangles[i + 1]], vertices[triangles[i + 2]] - vertices[triangles[i]]).z < 0)
            {
                int tmp = triangles[i];
                triangles[i] = triangles[i + 2];
                triangles[i + 2] = tmp;
            }
        }
    }

    public void OnDuplicate(string assetsPath)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh = meshFilter.sharedMesh;
        uniqueMeshPath = AssetDatabase.GenerateUniqueAssetPath(assetsPath + "Meshes/Polygon.asset");
        AssetDatabase.CreateAsset(meshFilter.sharedMesh, uniqueMeshPath);

        uniqueMaterial = new Material(vertexColorShader);
        customMaterial = uniqueMaterial;
        renderer.sharedMaterial = uniqueMaterial;
        uniqueMaterialPath = AssetDatabase.GenerateUniqueAssetPath(assetsPath + "Materials/Material.mat");
        AssetDatabase.CreateAsset(renderer.sharedMaterial, uniqueMaterialPath);
    }

    public void FreeAssets()
    {
        GetComponent<MeshFilter>().sharedMesh = null;
        GetComponent<MeshRenderer>().sharedMaterial = null;

        AssetDatabase.DeleteAsset(uniqueMaterialPath);
        AssetDatabase.DeleteAsset(uniqueMeshPath);
    }
}

#endif

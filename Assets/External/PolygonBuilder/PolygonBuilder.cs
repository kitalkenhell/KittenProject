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

    public bool lockPolygon = false;

    public string uniqueMaterialPath = "";
    public string uniqueMeshPath = "";

    bool inEditMode = false;
    bool enteringPlayMode = false;
    bool editModeCallbackAdded = false;
    bool updateCallbackAdded = false;

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

        if ( renderMode != RenderMode.customMaterial && (renderer.sharedMaterial == null || string.IsNullOrEmpty(uniqueMaterialPath)))
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

        triangles.AddRange(new int[]  { 3,1,0, 3,2,1 });

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

        uvs.Clear();
        uvs.Capacity = vertices.Count;
        for (int i = 0; i < vertices.Count; ++i)
        {
            uvs.Add(vertices[i] * uvScale + uvOffset);
        }

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

        polygon.vertices = vertices.ToArray();
        polygon.uv = uvs.ToArray();
        polygon.colors = colors.ToArray();
        polygon.triangles = triangles.ToArray();
    }

    void OnEnable()
    {
        if (!editModeCallbackAdded)
        {
            editModeCallbackAdded = true;
            UnityEditor.EditorApplication.playmodeStateChanged += EditModeCallback;
        }

        if (!updateCallbackAdded)
        {
            updateCallbackAdded = true;
            UnityEditor.EditorApplication.update += UpdateCallback;
        }
    }


    private void EditModeCallback()
    {
        if (!Application.isPlaying && inEditMode)
        {
            enteringPlayMode = true;
        }
    }


    private void UpdateCallback()
    {

        if (inEditMode == Application.isPlaying && !enteringPlayMode)
        {
            inEditMode = !inEditMode;
        }
        else if (enteringPlayMode)
        {
            inEditMode = false;
        }
    }


    void OnDestroy()
    {
        if (inEditMode)
        {
            GetComponent<MeshFilter>().sharedMesh = null;
            GetComponent<MeshRenderer>().sharedMaterial = null;

            AssetDatabase.DeleteAsset(uniqueMaterialPath);
            AssetDatabase.DeleteAsset(uniqueMeshPath);
        }
    }
}

#endif

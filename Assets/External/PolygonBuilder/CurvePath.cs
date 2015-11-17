using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CurvePath : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public List<Curve> curves = new List<Curve>();
    public int quality = 20;

    public bool fill;
    public bool addCollider;
    public bool showPoints = true;
    public bool showTangents = true;
    public bool useTansformTool = false;
    public float handleScale = 1;

    public float length;
    public bool hasAsset = false;
    public string uniqueMeshPath;

    List<Vector2> vertices2d;

    public void Refresh()
    {
        length = 0;

        foreach (Curve curve in curves)
        {
            length += curve.lenght;
        }

        vertices2d = new List<Vector2>();
        
        for (float d = 0; d <= length; d += length / curves.Count / quality)
        {
            vertices2d.Add(PointOnPath(d));
        }

        if (fill)
        {
            Fill();
            
            if (addCollider)
            {
                BuildCollider();
            }

        }
        else
        {
            Renderer meshRenderer = GetComponent<Renderer>();

            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    public void Reset()
    {
        Curve curve = new Curve();

        points.Clear();
        curves.Clear();

        curve.begin = Vector3.zero;
        curve.end = Vector3.up;
        curve.tangentBegin = Vector3.left;
        curve.tangentEnd = Vector3.right;

        points.Add(curve.begin);
        points.Add(curve.end);
        curves.Add(curve);
    }

    public void AddPoint()
    {
        if (points.Count >= 2)
        {
            Curve curve = new Curve();

            curve.begin = points[points.Count - 1];
            curve.end = points[points.Count - 1] + points[points.Count - 1] - points[points.Count - 2];
            curve.tangentBegin = Vector3.left;
            curve.tangentEnd = Vector3.right;

            points.Add(curve.end);
            curves.Add(curve);
        }
    }

    public Vector3 PointOnPath(float distance)
    {
        float distanceToGo = distance;
        int index = 0;

        if (distance >= length)
        {
            return points[points.Count - 1];
        }
        else if (distance <= 0)
        {
            return points[index];
        }

        while (distanceToGo > 0 && index < curves.Count)
        {
            distanceToGo -= curves[index].lenght;
            ++index;
        }
        --index;

        return curves[index].PointOnCurve(Mathf.Abs(distanceToGo));
    }

    public void Fill()
    {
        if (!hasAsset)
        {
            GenerateMeshAsset(); 
        }

        int[] triangles;
        Vector3[] vertices = new Vector3[vertices2d.Count];
        Mesh mesh = new Mesh();

        triangles = new Triangulator(vertices2d.ToArray()).Triangulate();
        
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2d[i].x, vertices2d[i].y, 0);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void BuildCollider()
    {
        PolygonCollider2D collider2d = GetComponent<PolygonCollider2D>();

        if (collider2d == null)
        {
            collider2d = gameObject.AddComponent<PolygonCollider2D>();
        }

        collider2d.SetPath(0, vertices2d.ToArray());
    }

    void GenerateMeshAsset()
    {
        string assetsPath = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this)).Replace("CurvePath.cs", "");

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
            uniqueMeshPath = AssetDatabase.GenerateUniqueAssetPath(assetsPath + "Meshes/Polygon.asset");
            meshFilter.sharedMesh = new Mesh();
            meshFilter.sharedMesh = meshFilter.sharedMesh;
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, uniqueMeshPath);
        }

        hasAsset = true;
    }

    public void FreeMeshAsset()
    {
        GetComponent<MeshFilter>().sharedMesh = null;
        AssetDatabase.DeleteAsset(uniqueMeshPath);
    }
}

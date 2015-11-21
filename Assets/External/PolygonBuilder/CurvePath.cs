#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurvePath : MonoBehaviour
{
    public float length;
    public List<Vector3> points = new List<Vector3>();
    public List<Curve> curves = new List<Curve>();

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

#if UNITY_EDITOR
    public const int minVertexCountToFill = 2;

    public int quality = 20;
    public bool fill = false;
    public bool loop = false;
    public bool addCollider;
    public bool showPoints = true;
    public bool showTangents = true;
    public bool useTansformTool = false;
    public float handleScale = 1;

    
    public bool hasAsset = false;
    public string uniqueMeshPath;
    public Vector3 edgePoint;
    public List<Vector2> vertices2d = new List<Vector2>();

    public void Refresh()
    {
        length = 0;

        if (curves.Count < minVertexCountToFill)
        {
            fill = loop = false;
        }

        if (loop)
        {
            curves[curves.Count - 1].end = curves[0].begin;
        }

        foreach (Curve curve in curves)
        {
            length += curve.lenght;
        }

        vertices2d.Clear();

        for (int i = 0; i < curves.Count; ++i)
        {
            for (int j = 0; j <= curves[i].quality - 1; ++j)
            {
                vertices2d.Add(curves[i].PointOnCurve(curves[i].lenght - curves[i].lenght * ((float)j / quality)));
            }
        }

        vertices2d.Add(curves[curves.Count - 1].end);


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

    public void Fill()
    {
        if (!hasAsset)
        {
            GenerateMeshAsset(); 
        }

        int[] triangles;
        Vector3[] vertices = new Vector3[vertices2d.Count];
        Mesh mesh = new Mesh();

        triangles = new Triangulator(vertices2d.GetRange(0, vertices2d.Count - 1).ToArray()).Triangulate();
        
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

        collider2d.SetPath(0, vertices2d.GetRange(0, vertices2d.Count - 1).ToArray());
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

    public void AddPoint()
    {
        int closest = -1;
        float closestDistance = Mathf.Infinity;
        int curveIndex;
        Curve curve = new Curve();

        for (int i = 0; i < vertices2d.Count - 1; i++)
        {
            float distance = HandleUtility.DistanceToLine(transform.TransformPoint(vertices2d[i]), transform.TransformPoint(vertices2d[i + 1]));

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = i;
            }
        }

        curveIndex = closest / quality;

        if (closest >= 0 )
        {
            edgePoint = transform.InverseTransformPoint(HandleUtility.ClosestPointToPolyLine(new Vector3[] { transform.TransformPoint(vertices2d[closest]), transform.TransformPoint(vertices2d[closest + 1]) }));
        }

        curve.begin = edgePoint;

        if (curveIndex == curves.Count - 1)
        {
            curve.end = curves[curveIndex].end;
            curves.Add(curve);
            points.Add(curve.begin);
        }
        else
        {
            curve.end = curves[curveIndex + 1].begin;
            curves.Insert(curveIndex + 1, curve);
            points.Insert(curveIndex + 1, curve.begin);
        }

        curves[curveIndex].end = edgePoint;
    }

    public void CenterPivot()
    {
        Vector3 mean = Vector3.zero;

        foreach (var curve in curves)
        {
            mean += curve.begin;
        }

        mean /= curves.Count;

        for (int i = 0; i < curves.Count; i++)
        {
            curves[i].begin -= mean;
            curves[i].end -= mean;
        }

        Refresh();
    }

    public void FreeMeshAsset()
    {
        GetComponent<MeshFilter>().sharedMesh = null;
        AssetDatabase.DeleteAsset(uniqueMeshPath);
    }

 #endif
}


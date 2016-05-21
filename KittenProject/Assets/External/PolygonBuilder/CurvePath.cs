#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CurvePath : MonoBehaviour
{
    const float eps = 0.01f;

    public bool loop = false;
    public float length;
    public List<Vector3> points = new List<Vector3>();
    public List<Curve> curves = new List<Curve>();

    public Vector3 PointOnPathLocal(float distance, bool useCurveFastApproximation = true)
    {
        int index = 0;

        if (distance >= length)
        {
            return points[points.Count - 1];
        }
        else if (distance <= 0)
        {
            return points[index];
        }

        while (distance > 0 && index < curves.Count)
        {
            distance -= curves[index].lenght;
            ++index;
        }
        --index;

        if (useCurveFastApproximation)
        {
            return curves[index].PointOnCurveFast(Mathf.Abs(distance));
        }
        else
        {
            return curves[index].PointOnCurve(Mathf.Abs(distance));
        }
    }

    public Vector3 PointOnPathWorld(float distance, bool useCurveFastApproximation = true)
    {
        return transform.TransformPoint(PointOnPathLocal(distance, useCurveFastApproximation));
    }

#if UNITY_EDITOR
    public const int minVertexCountToFill = 2;

    public int quality = 20;
    public bool fill = false;
    public bool createOutline = false;
    public float outlineOffset = 0.001f;
    public bool extrude = false;
    public float extrudeScale = 1;
    public bool worldSpaceUvs = false;
    public bool addCollider;
    public bool showPoints = true;
    public bool showTangents = true;
    public bool useTansformTool = false;
    public float handleScale = 1;

    public CurvePath outlineChild;

    public List<Vector2> vertices2d = new List<Vector2>();

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            meshFilter.sharedMesh = new Mesh();
        }

        Refresh();
    }

    public void Refresh()
    {
        length = 0;

        if (curves.Count == 0)
        {
            return;
        }

        if (curves.Count < minVertexCountToFill)
        {
            fill = loop = false;
        }

        if (loop)
        {
            if (fill || extrude)
            {
                curves[curves.Count - 1].end = curves[0].begin + (curves[0].begin - curves[0].path[1]) * eps;
            }
            else
            {
                curves[curves.Count - 1].end = curves[0].begin;
            }
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
                vertices2d.Add(curves[i].PointOnCurveFast(curves[i].lenght - curves[i].lenght * ((float)j / quality)));
            }
        }

        vertices2d.Add(curves[curves.Count - 1].end);


        if (fill)
        {
            Fill();
            extrude = false;

            if (addCollider)
            {
                BuildCollider();
            }

        }
        else if (extrude)
        {
            Extrude();
            fill = false;
            createOutline = false;

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

            createOutline = false;
        }

        Outline();
    }

    public void Reset()
    {
        Curve curve = new Curve();

        points.Clear();
        curves.Clear();

        curve.begin = Vector3.zero;
        curve.end = Vector3.up;
        curve.beginTangent = Vector3.left;
        curve.endTangent = Vector3.right;

        points.Add(curve.begin);
        points.Add(curve.end);
        curves.Add(curve);
    }

    public void Fill()
    {
        CheckRenderer();

        int[] triangles;
        Vector3[] vertices = new Vector3[vertices2d.Count];
        Vector2[] uvs = vertices2d.ToArray();
        
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        triangles = new Triangulator(vertices2d.GetRange(0, loop ? vertices2d.Count - 1 : vertices2d.Count).ToArray()).Triangulate();

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2d[i].x, vertices2d[i].y, 0);
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Extrude()
    {
        CheckRenderer();

        int[] triangles;
        List<int> trianglesList = new List<int>();
        Vector3[] vertices;
        Vector2[] uvs;
        Vector2 normal;
        List<Vector2> path = new List<Vector2>(); 

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        for (int i = 0; i < vertices2d.Count; ++i)
        {
            path.Add(vertices2d[i]);
        }

        normal = Utils.PerpendicularVector2(vertices2d[vertices2d.Count - 1] - vertices2d[vertices2d.Count - 2]).normalized * extrudeScale;

        path.Add(vertices2d[vertices2d.Count - 1] + normal);

        for (int i = vertices2d.Count - 2; i > 0 ; --i)
        {
            normal = Utils.PerpendicularVector2((vertices2d[i] - vertices2d[i - 1] + vertices2d[i+1] - vertices2d[i]) / 2.0f).normalized * extrudeScale;
            path.Add(vertices2d[i] + normal);
        }

        normal = Utils.PerpendicularVector2(vertices2d[1] - vertices2d[0]).normalized * extrudeScale;
        path.Add(vertices2d[0] + normal);

        int n = path.Count - 1;


        if (extrudeScale > 0) //different vertex index order 
        {
            for (int i = 0; i < vertices2d.Count - 1; ++i)
            {
                trianglesList.Add(n - i);
                trianglesList.Add(i + 1);
                trianglesList.Add(i);

                trianglesList.Add(n - i - 1);
                trianglesList.Add(i + 1);
                trianglesList.Add(n - i);
            }

            if (loop)
            {
                trianglesList.Add(vertices2d.Count);
                trianglesList.Add(path.Count - 1);
                trianglesList.Add(0);

            }
        }
        else
        {
            for (int i = 0; i < vertices2d.Count - 1; ++i)
            {
                trianglesList.Add(i);
                trianglesList.Add(i + 1);
                trianglesList.Add(n - i);

                trianglesList.Add(n - i);
                trianglesList.Add(i + 1);
                trianglesList.Add(n - i - 1);
            }

            if (loop)
            {
                trianglesList.Add(path.Count - 1);
                trianglesList.Add(0);
                trianglesList.Add(vertices2d.Count);
            }
        }

        triangles = trianglesList.ToArray();

        vertices = new Vector3[path.Count];
        uvs = new Vector2[path.Count];

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(path[i].x, path[i].y, 0);
        }

        if (worldSpaceUvs)
        {
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector3(path[i].x, path[i].y, 0);
            }
        }
        else
        {
            float uBias = 0.1f;
            float v = 0;

            uvs[0] = Vector2.zero;

            for (int i = 1; i < vertices2d.Count; ++i)
            {
                v += Vector2.Distance(path[i], path[i - 1]) / extrudeScale;
                uvs[i] = new Vector2(uBias, v);
            }

            uvs[vertices2d.Count] = new Vector2(1, v);

            int j = 1;
            for (int i = vertices2d.Count + 1; i < path.Count; ++i)
            {
                v -= Vector2.Distance(path[j], path[j - 1]) / extrudeScale;
                uvs[i] = new Vector2(1f - uBias, v);
                ++j;
            } 
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Outline()
    {
        if (createOutline)
        {
            if (outlineChild == null)
            {
                outlineChild = (Instantiate(gameObject) as GameObject).GetComponent<CurvePath>();
                
                //Destroy all components but transform, curve path, mesh renderer, mesh filter
            }

            outlineChild.transform.parent = transform;
            outlineChild.transform.localPosition = Vector3.forward * outlineOffset;
            outlineChild.transform.localScale = Vector3.one;
            outlineChild.transform.localRotation = Quaternion.identity;
            outlineChild.name = gameObject.name + "Outline";

            outlineChild.createOutline = false;
            outlineChild.fill = false;
            outlineChild.extrude = true;
            outlineChild.worldSpaceUvs = worldSpaceUvs;
            outlineChild.extrudeScale = extrudeScale;
            outlineChild.loop = loop;
            outlineChild.quality = quality;
            outlineChild.points = points;
            outlineChild.curves = curves;
            outlineChild.showPoints = false;
            outlineChild.showTangents = false;
            outlineChild.useTansformTool = false;

            outlineChild.Refresh();
        }
        else if (outlineChild != null)
        {
            DestroyImmediate(outlineChild.gameObject);
            outlineChild = null;
        }
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

    void CheckRenderer()
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
        }
    }

    public void AddPoint()
    {
        int closest = -1;
        float closestDistance = Mathf.Infinity;
        int curveIndex;
        Vector3 edgePoint;
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
        edgePoint = transform.InverseTransformPoint(HandleUtility.ClosestPointToPolyLine(new Vector3[] { transform.TransformPoint(vertices2d[closest]), transform.TransformPoint(vertices2d[closest + 1]) }));
        curve.begin = edgePoint;

        if (curveIndex == curves.Count - 1)
        {
            curve.end = curves[curveIndex].end;
            curve.beginTangent = (curve.end - curve.begin) / 2.0f;
            curve.endTangent = -curves[curveIndex].beginTangent;

            curves.Add(curve);
            points.Add(curve.begin);
        }
        else
        {
            curve.end = curves[curveIndex + 1].begin;
            curve.beginTangent = (curve.end - curve.begin) / 2.0f;
            curve.endTangent = -curves[curveIndex + 1].beginTangent;

            curves.Insert(curveIndex + 1, curve);
            points.Insert(curveIndex + 1, curve.begin);
        }

        curves[curveIndex].end = edgePoint;
    }

    public void RemovePoint(Vector2 mousePosition)
    {
        const int extraEditorSpace = 35; //wtf?

        int closest = -1;
        float closestDistance = Mathf.Infinity;

        if (points.Count <= 2)
        {
            return;
        }

        mousePosition.y = Screen.height - mousePosition.y - extraEditorSpace;

        for (int i = 0; i < points.Count - 1; i++)
        {
            float distance = Vector3.Distance(transform.TransformPoint(points[i]), Camera.current.ScreenToWorldPoint(mousePosition));

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = i;
            }
        }

        points.RemoveAt(closest);

        if (closest == 0)
        {
            curves.RemoveAt(0);
            if (loop)
            {
                curves[curves.Count - 1].end = curves[0].begin;
            }
        }
        else if (closest == points.Count - 1)
        {
            --closest;
            curves.RemoveAt(curves.Count - 1);

            if (loop)
            {
                curves[curves.Count - 1].end = curves[0].begin; 
            }
        }
        else
        {
            curves[closest - 1].end = curves[closest].end;
            curves.RemoveAt(closest);
        }
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

 #endif
}


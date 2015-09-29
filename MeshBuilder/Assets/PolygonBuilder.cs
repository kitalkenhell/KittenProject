using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PolygonBuilder : MonoBehaviour
{
    public enum SelectionMode
    {
        vertex,
        face
    };

    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Color> colors = new List<Color>();
    public List<int> triangles = new List<int>();

    public SelectionMode selectionMode;
    public List<int> selection = new List<int>();

    public float uvScale = 1;
    public Vector3 uvOffset = Vector3.zero;

    public void BuildQuad()
    {
        Mesh polygon = GetComponent<MeshFilter>().sharedMesh;

        polygon.Clear();

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

        polygon.vertices = vertices.ToArray();
        polygon.colors = colors.ToArray();
        polygon.triangles = triangles.ToArray();



        selection.Clear();
    }

    public void Refresh()
    {
        Mesh polygon = GetComponent<MeshFilter>().sharedMesh;

        polygon.Clear();

        uvs.Clear();
        uvs.Capacity = vertices.Count;
        for (int i = 0; i < vertices.Count; ++i)
        {
            uvs.Add(vertices[i] * uvScale + uvOffset);
        }

        polygon.vertices = vertices.ToArray();
        polygon.uv = uvs.ToArray();
        polygon.colors = colors.ToArray();
        polygon.triangles = triangles.ToArray();
    }
}


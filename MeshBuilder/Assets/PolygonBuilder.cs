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
    public List<Color> colors = new List<Color>();
    public List<int> triangles = new List<int>();

    public SelectionMode selectionMode;
    public List<int> selection = new List<int>();

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

        polygon.vertices = vertices.ToArray();
        polygon.colors = colors.ToArray();
        polygon.triangles = triangles.ToArray();
    }
}


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

    public Vector3[] vertices;
    public Color[] colors;
    public int[] triangles;

    public SelectionMode selectionMode;
    public List<int> selection = new List<int>();

    public void BuildQuad()
    {
        Mesh polygon = GetComponent<MeshFilter>().sharedMesh;

        colors = new Color[] 
        {
            new Color(1,1,1,1),
            new Color(1,1,1,1),
            new Color(1,1,1,1),
            new Color(1,1,1,1)
        };

        colors = new Color[] 
        {
            new Color(Random.value,Random.value,Random.value,1),
            new Color(Random.value,Random.value,Random.value,1),
            new Color(Random.value,Random.value,Random.value,1),
            new Color(Random.value,Random.value,Random.value,1)
        };

        vertices = new Vector3[] 
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, 0.5f, 0),
        };

        triangles = new int[] 
        {
            3,1,0,
            3,2,1
        };

        polygon.vertices = vertices;
        polygon.colors = colors;
        polygon.triangles = triangles;

        selection.Clear();
    }

    public void Refresh()
    {
        Mesh polygon = GetComponent<MeshFilter>().sharedMesh;

        polygon.vertices = vertices;
        polygon.colors = colors;
        polygon.triangles = triangles;
    }
}


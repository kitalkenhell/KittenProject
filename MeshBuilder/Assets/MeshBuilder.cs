using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshBuilder : MonoBehaviour
{
    public Vector3[] vertices;
    public Color[] colors;
    public int[] triangles;

    public List<int> selectedVertices = new List<int>();

    public void BuildQuad()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

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

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;

        selectedVertices.Clear();
    }

    public void Refresh()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;

        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
    }
}

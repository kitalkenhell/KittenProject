using UnityEngine;
using System.Collections;

public class DrawGizmo : MonoBehaviour
{
    public Color color = Color.white;
    public float size = 1.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(size, size, size));
    }
}

using UnityEngine;
using System.Collections;

public class MovablePlatformWaypoint : MonoBehaviour 
{
    public float speed = 10.0f;
    public float acceleration = 2.0f;
    public float stoppingDistance = 2.0f;
    public float waitTime = 0.0f;

    void OnDrawGizmos()
    {
        const float size = 1.5f;

        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, new Vector3(size, size, size));
    }
}

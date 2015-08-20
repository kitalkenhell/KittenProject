using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;

    public Transform hellfire;
    public float minDistanceFromHellfire;

    void LateUpdate()
    {
        transform.position = target.position + offset;

        if (transform.position.y - minDistanceFromHellfire < hellfire.position.y)
        {
            transform.SetPositionY(hellfire.position.y + minDistanceFromHellfire);
        }
	}
}

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = target.position + offset;
	}
}

using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

	void Update () 
    {
        transform.position = target.position + offset;
	}
}

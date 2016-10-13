using UnityEngine;
using System.Collections;

public class FollowScale : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset = Vector3.zero;

	void Update () 
    {
        transform.localScale = target.lossyScale + offset;
	}
}

using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;

    public Transform hellfire;
    public float minDistanceFromHellfire;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        PostOffice.victory += OnVictory;
    }

    void OnDestroy()
    {
        PostOffice.victory -= OnVictory;
    }

    void OnVictory()
    {
        animator.enabled = true;
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;

        if (hellfire != null)
        {
            if (transform.position.y - minDistanceFromHellfire < hellfire.position.y)
            {
                transform.SetPositionY(hellfire.position.y + minDistanceFromHellfire);
            } 
        }
	}
}

using UnityEngine;
using System.Collections;

public class CutsceneCharacterController : MonoBehaviour
{
    public Transform[] waypoints;
    public float smoothTime;
    public float maxSpeed;

    Animator animator;

    int speedAnimHash;

    int currentWaypoint;
    Vector3 velocity;

    public bool HasReachedCurrentPoint
    {
        get
        {
            const float threshold = 1.0f;
            return Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < threshold;
        }
    }

    public float DistanceToCurrentPoint
    {
        get
        {
            return Vector3.Distance(transform.position, waypoints[currentWaypoint].position);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();

        speedAnimHash = Animator.StringToHash("Speed");

        currentWaypoint = 0;
    }

    void Update()
    {
        if (waypoints == null || currentWaypoint < 0 || currentWaypoint >= waypoints.Length)
        {
            enabled = false;
        }
        else
        {
            animator.SetFloat(speedAnimHash, velocity.magnitude);
            transform.position = Vector3.SmoothDamp(transform.position, waypoints[currentWaypoint].position, ref velocity, smoothTime, maxSpeed);
        }
    }

    public void MoveToNextPoint()
    {
        ++currentWaypoint;
    }
}

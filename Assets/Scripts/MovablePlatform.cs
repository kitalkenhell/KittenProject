using UnityEngine;
using System.Collections;

public class MovablePlatform : MonoBehaviour 
{
    enum States
    {
        wait,
        move
    }

    const float nextWaypintThreshold = 0.05f;

    public Waypoint[] waypoints;
    public bool looping;

    Rigidbody2D body;

    int currentWaypoint;
    bool moveForward;
    float speed;
    Vector2 velocity;
    States state;

    Vector3 displacement;

	void Start () 
    {
        body = GetComponent<Rigidbody2D>();

        currentWaypoint = 0;
        transform.position = waypoints[currentWaypoint].transform.position;
        moveForward = true;
        speed = 0;
        velocity = Vector2.zero;
        state = States.move;

        if (waypoints[currentWaypoint].waitTime > 0)
        {
            StartCoroutine(Wait(waypoints[currentWaypoint].waitTime));
        }
	}
	
	void FixedUpdate() 
    {
        Waypoint current = waypoints[currentWaypoint];
        float distance = Vector3.Distance(transform.position, current.transform.position);

        if (state == States.wait)
        {
            return;
        }

        if (distance < nextWaypintThreshold)
        {
            SetNextWaypoint();
            return;
        }

        if (distance < current.stoppingDistance)
        {
            speed = current.speed * distance / current.stoppingDistance;
        }
        else
        {
            if (speed < current.speed)
            {
                speed += current.acceleration;
            }
            if (speed > current.speed)
            {
                speed = current.speed;
            }
        }

        velocity = (current.transform.position - transform.position).normalized * speed;
        displacement = velocity * Time.fixedDeltaTime;

        if (displacement.magnitude > distance)
        {
            displacement = current.transform.position - transform.position;
        }

        body.MovePosition(transform.position + displacement);
	}

    int SetNextWaypoint()
    {
        if (waypoints[currentWaypoint].waitTime > 0)
        {
            StartCoroutine(Wait(waypoints[currentWaypoint].waitTime));
        }

        if (looping)
        {
            if (++currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
        else
        {

            if (moveForward)
            {
                if (++currentWaypoint >= waypoints.Length)
                {
                    moveForward = !moveForward;
                    currentWaypoint = waypoints.Length - 2;
                }
            }
            else
            {
                if (--currentWaypoint < 0)
                {
                    moveForward = !moveForward;
                    currentWaypoint = 1;
                }
            }
        }

        return currentWaypoint;
    }

    IEnumerator Wait(float time)
    {
        body.velocity = Vector2.zero;
        state = States.wait;
        yield return new WaitForSeconds(time);
        state = States.move;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        MovablePlatformEffector effector = collision.gameObject.GetComponent<MovablePlatformEffector>();
        
        if (effector != null)
        {
            effector.onEnter();
            OnCollisionStay2D(collision);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        const int contactPointIndex = 0;
        const float ignoreGroundCollisionThreshold = 0.01f;

        MovablePlatformEffector effector = collision.gameObject.GetComponent<MovablePlatformEffector>();

        if (effector != null)
        {
            Vector2 normal = collision.contacts[contactPointIndex].normal;

            if (Vector2.Dot(normal, Vector2.up) < ignoreGroundCollisionThreshold)
            {
                effector.onMoved(displacement);
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        MovablePlatformEffector effector = collision.gameObject.GetComponent<MovablePlatformEffector>();

        if (effector != null)
        {
            effector.onExit();
        }
    }
}
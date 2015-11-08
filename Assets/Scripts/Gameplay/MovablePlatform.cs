using UnityEngine;
using System.Collections;

public class MovablePlatform : MonoBehaviour 
{
    enum States
    {
        wait,
        move
    }

    const float nextWaypintThreshold = 0.1f;

    public Waypoint[] waypoints;
    public bool looping;

    Rigidbody2D body;

    int currentWaypoint;
    bool moveForward;
    float speed;
    Vector2 velocity;
    States state;
    Vector2 lastFramePosition;
    Vector2 displacement;
    Vector2 lastFrameDisplacement;

    public Vector2 LastFrameDisplacement
    {
        get
        {
            return lastFrameDisplacement;
        }
    }

	public void Start () 
    {
        body = GetComponent<Rigidbody2D>();

        currentWaypoint = 0;
        transform.SetPositionXY(waypoints[currentWaypoint].transform.position);
        lastFramePosition = transform.position;
        moveForward = true;
        speed = 0;
        velocity = Vector2.zero;
        state = States.move;

        if (waypoints[currentWaypoint].waitTime > 0)
        {
            StartCoroutine(Wait(waypoints[currentWaypoint].waitTime));
        }
	}

    public void Update()
    {
        lastFrameDisplacement = transform.position.XY() - lastFramePosition;
        lastFramePosition = transform.position;
    }

    public void FixedUpdate() 
    {

        Waypoint current = waypoints[currentWaypoint];
        float distance = Vector2.Distance(transform.position, current.transform.position);

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

        body.MovePosition(transform.position.XY() + displacement);
	}

    public int SetNextWaypoint()
    {
        speed = 0;
        displacement = Vector3.zero;

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

    public IEnumerator Wait(float time)
    {
        displacement = Vector3.zero;
        body.velocity = Vector2.zero;
        speed = 0;
        state = States.wait;
        yield return new WaitForSeconds(time);
        state = States.move;
    }
}
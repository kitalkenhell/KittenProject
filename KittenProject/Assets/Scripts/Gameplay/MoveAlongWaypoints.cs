using UnityEngine;
using System.Collections;

public class MoveAlongWaypoints : MonoBehaviour 
{
    enum States
    {
        wait,
        move,
        pause
    }

    const float nextWaypintThreshold = 0.4f;

    public Waypoint[] waypoints;
    public bool looping;

    public Vector2 Velocity
    {
        private set;
        get;
    }

    public Vector2 LastFrameDisplacement
    {
        private set;
        get;
    }

    Rigidbody2D body;

    int currentWaypoint;
    bool moveForward;
    float speed;

    States state;
    Vector2 lastFramePosition;
    Vector2 displacement;

	public void Start () 
    {
        body = GetComponent<Rigidbody2D>();

        currentWaypoint = 0;
        transform.SetPositionXY(waypoints[currentWaypoint].transform.position);
        lastFramePosition = transform.position;
        moveForward = true;
        speed = 0;
        Velocity = Vector2.zero;
        state = States.move;

        if (waypoints[currentWaypoint].waitTime > 0)
        {
            StartCoroutine(Wait(waypoints[currentWaypoint].waitTime));
        }
	}

    public void Update() 
    {
        LastFrameDisplacement = transform.position.XY() - lastFramePosition;
        lastFramePosition = transform.position;

        Waypoint current = waypoints[currentWaypoint];
        float distance = Vector2.Distance(transform.position.XY(), current.transform.position.XY());

        if (state == States.wait)
        {
            return;
        }

        if (distance < nextWaypintThreshold)
        {
            if (state != States.pause)
            {
                SetNextWaypoint(); 
            }
            return;
        }

        if (distance < current.stoppingDistance)
        {
            speed = current.speed * distance / current.stoppingDistance;
        }
        else if (state == States.pause)
        {
            speed = Mathf.Max(speed - current.acceleration, 0);
        }
        else
        {
            speed = Mathf.Min(speed + current.acceleration, current.speed);
        }

        Velocity = (current.transform.position.XY() - transform.position.XY()).normalized * speed;
        displacement = Velocity * Time.deltaTime;

        if (displacement.magnitude > distance)
        {
            displacement = current.transform.position - transform.position;
        }

        body.MovePosition(transform.position.XY() + displacement);
    }

    public void Pause(bool pause)
    {
        if (pause && state != States.pause)
        {
            state = States.pause;
        }
        else if (!pause && state == States.pause)
        {
            state = States.move;
        }
    }

    public int SetNextWaypoint()
    {
        if (!Mathf.Approximately(waypoints[currentWaypoint].stoppingDistance, 0))
        {
            speed = 0; 
        }
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
        Velocity = Vector2.zero;
        body.velocity = Vector2.zero;
        speed = 0;
        state = States.wait;
        yield return new WaitForSeconds(time);

        if (state != States.pause)
        {
            state = States.move; 
        }
    }
}
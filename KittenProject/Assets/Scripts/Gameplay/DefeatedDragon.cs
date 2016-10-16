using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedDragon : MonoBehaviour
{
    public Transform[] waypoints;
    public int pauseWaypointIndex;
    public float maxSpeed;
    public float smoothTime;
    public SpeechBalloon speechBalloon;

    public bool MoveToTheEnd
    {
        get
        {
            return moveToTheEnd;
        }
        set
        {
            speechBalloon.Show(false);
            moveToTheEnd = value;
        }
    }

    Vector2 velocity;
    float speed;
    int nextPointIndex;
    float zPosition;
    bool moveToTheEnd;

    void Start()
    {
        speed = maxSpeed;
        zPosition = transform.position.z;
        MoveToTheEnd = false;
        speechBalloon.Show();
    }

    void Update()
    {
        const float nextPointThreshold = 0.5f;
        const float speedThreshold = 1.5f;

        if (nextPointIndex < 0 || nextPointIndex >= waypoints.Length)
        {
            enabled = false;
            return;
        }

        if (nextPointIndex > pauseWaypointIndex && !MoveToTheEnd)
        {
            return;
        }

        if (nextPointIndex != pauseWaypointIndex && nextPointIndex != waypoints.LastIndex() && speed > maxSpeed - speedThreshold)
        {
            Vector2 lastFramePosition = transform.position.XY();
            transform.position = Vector2.MoveTowards(transform.position.XY(), waypoints[nextPointIndex].position.XY(), speed * Time.deltaTime);
            velocity = (transform.position.XY() - lastFramePosition) / Time.deltaTime;
            speed = maxSpeed;
        }
        else
        {
            transform.position = Vector2.SmoothDamp(transform.position.XY(), waypoints[nextPointIndex].position.XY(), ref velocity, smoothTime, maxSpeed, Time.deltaTime);
        }

        if (Vector2.Distance(transform.position.XY(), waypoints[nextPointIndex].position.XY()) < nextPointThreshold)
        {
            ++nextPointIndex;
        }

        transform.SetScaleX(Mathf.Abs(transform.localScale.x) * -Mathf.Sign(velocity.x));
        transform.SetPositionZ(zPosition);
    }

    public void GoToFinalWaypoint()
    {
        nextPointIndex = waypoints.LastIndex();
    }
}

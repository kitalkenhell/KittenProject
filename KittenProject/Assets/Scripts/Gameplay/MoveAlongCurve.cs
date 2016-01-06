using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveAlongCurve : MonoBehaviour
{
    public CurvePath path;
    public float speed;


    Rigidbody2D body;
    float traveledDistance;
    bool moveForward;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();

        traveledDistance = 0;
        moveForward = true;
    }

    public void FixedUpdate()
    {
        if (moveForward)
        {
            traveledDistance += speed * Time.fixedDeltaTime;

            if (traveledDistance > path.length)
            {
                if (!path.loop)
                {
                    moveForward = !moveForward;
                }
                else
                {
                    traveledDistance -= path.length;
                }
            }
        }
        else
        {
            traveledDistance -= speed * Time.fixedDeltaTime;

            if (traveledDistance < 0)
            {
                if (!path.loop)
                {
                    moveForward = !moveForward;
                }
                else
                {
                    traveledDistance -= path.length;
                }
            }
        }

        body.MovePosition(path.PointOnPathWorld(traveledDistance));
    }
}

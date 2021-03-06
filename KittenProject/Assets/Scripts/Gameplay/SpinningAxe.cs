﻿using UnityEngine;
using System.Collections;

public class SpinningAxe : MonoBehaviour 
{
    public AnimationCurve positionCurve;
    public float maxAngle = 45.0f;
    public float frequency = 1.0f;
    public float rotationOffset = 0;
    public float timeOffset = 0;
    public Vector2 forceMultiplier;
    public Vector2 force;
    public Vector2 minPushForce;
    public float disableControlsDuraton;

    Rigidbody2D body;
    float speed = 0;
    float lastRotation = 0;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Vector2 pushForce = new Vector2(forceMultiplier.x * speed + force.x * Mathf.Sign(speed), forceMultiplier.y * Mathf.Abs(speed) + force.y);

            pushForce.x = Mathf.Max(pushForce.x, Mathf.Abs(minPushForce.x)) * Mathf.Sign(pushForce.x);
            pushForce.y = Mathf.Max(pushForce.y, Mathf.Abs(minPushForce.y)) * Mathf.Sign(pushForce.y);

            player.PushAndHit(pushForce, true, true, disableControlsDuraton);
        }
    }

    void Update()
    {
        float time = (Time.timeSinceLevelLoad + timeOffset) * frequency;
        float currentRotiation; 

        time = time - Mathf.Floor(time);
        currentRotiation = positionCurve.Evaluate(time) * maxAngle;
        speed = currentRotiation - lastRotation;
        lastRotation = currentRotiation;

        body.MoveRotation(currentRotiation + rotationOffset);
    }
}

﻿using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
{
    public Transform force;
    public float disableControlsDuration;

    bool active;

    void Start()
    {
        active = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        const float amplification = 3.0f;

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && active)
        {
            active = false;
            player.PushAndHit((force.position - transform.position).XY() * amplification, true, true, disableControlsDuration);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        active = true;
    }

}

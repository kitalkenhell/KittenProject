﻿using UnityEngine;
using System.Collections;

public class DynamicCoinResetter : MonoBehaviour 
{
    Vector3 coinScale;
    Rigidbody2D body;
    CoinMover mover;
    DynamicCoinTrigger trigger;

    public void Init()
    {
        coinScale = transform.localScale;
        mover = GetComponent<CoinMover>();
        body = GetComponent<Rigidbody2D>();
        trigger = GetComponentInChildren<DynamicCoinTrigger>();
        trigger.Init();
    }

    public void Reset(Vector3 position, Vector2 velocity, Transform target)
    {
        gameObject.SetActive(true);
        mover.target = target;
        transform.position = position;
        transform.localScale = coinScale;
        body.velocity = velocity;
        body.isKinematic = false;
        trigger.Reset();
        
    }
}

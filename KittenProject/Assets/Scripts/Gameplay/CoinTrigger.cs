﻿using UnityEngine;
using System.Collections;

public class CoinTrigger : MonoBehaviour 
{
    public AudioSource pickUpSound;

    CoinMover coinMover;
    bool collected;

    void Start()
    {
        coinMover = GetComponent<CoinMover>();
        coinMover.enabled = false;
        collected = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!collected)
        {
            collected = true;
            enabled = false;
            coinMover.enabled = true;
            pickUpSound.Play();
            pickUpSound.transform.parent = null;
        }
    }
}

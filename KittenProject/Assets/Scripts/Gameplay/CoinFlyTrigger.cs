using UnityEngine;
using System.Collections;

public class CoinFlyTrigger : MonoBehaviour 
{
    public AudioSource pickUpSound;
    public int amountOfCoins;

    Pickup pickupMover;
    MoveAlongCurve curveMover;
    bool collected;

    void Start()
    {
        pickupMover = GetComponent<Pickup>();
        curveMover = GetComponent<MoveAlongCurve>();

        pickupMover.enabled = false;
        collected = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!collected && other.gameObject.layer == Layers.Player)
        {
            collected = true;
            enabled = false;
            pickupMover.enabled = true;
            pickUpSound.Play();
            pickUpSound.transform.parent = null;
            curveMover.enabled = false;

            GetComponent<CoinEmitter>().BurstEmit(amountOfCoins);
        }
    }
}

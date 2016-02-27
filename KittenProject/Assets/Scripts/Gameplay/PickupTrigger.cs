using UnityEngine;
using System.Collections;

public class PickupTrigger : MonoBehaviour 
{
    public AudioSource pickUpSound;

    Pickup pickupMover;
    bool collected;

    void Start()
    {
        pickupMover = GetComponent<Pickup>();
        pickupMover.enabled = false;
        collected = false;
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        if (!collected && !(pickupMover.type == Pickup.Type.heart && CoreLevelObjects.player.Health >= GameSettings.MaxPlayerHealth))
        {
            collected = true;
            enabled = false;
            pickupMover.enabled = true;
            pickUpSound.Play();
            pickUpSound.transform.parent = null;
        }
    }
}

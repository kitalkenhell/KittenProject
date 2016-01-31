using UnityEngine;
using System.Collections;

public class DynamicCoinTrigger : MonoBehaviour 
{
    public float ActivationDelay = 1.0f;
    public AudioSource pickUpSound;

    Pickup pickupMover;
    BoxCollider2D boxCollider;
    bool collected;
    Rigidbody2D parentBody;
    BoxCollider2D parentCollider;

    public void Awake()
    {
        pickupMover = GetComponentInParent<Pickup>();
        boxCollider = GetComponent<BoxCollider2D>();
        parentBody = GetComponentInParent<Rigidbody2D>();
        parentCollider = GetComponentInParent<BoxCollider2D>();
    }

    public void Reset()
    {
        boxCollider.enabled = false;
        pickupMover.enabled = false;
        collected = false;
        StartCoroutine(Activator());
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!collected && other.gameObject.layer == Layers.Player)
        {
            pickUpSound.Play();
            pickUpSound.transform.parent = null;
            collected = true;
            enabled = false;
            parentBody.isKinematic = true;
            parentCollider.enabled = false;
            pickupMover.enabled = true; 
        }
    }

    IEnumerator Activator()
    {
        yield return new WaitForSeconds(ActivationDelay);
        boxCollider.enabled = true;
    }
}

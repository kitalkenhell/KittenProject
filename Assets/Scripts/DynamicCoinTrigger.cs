using UnityEngine;
using System.Collections;

public class DynamicCoinTrigger : MonoBehaviour 
{
    public float ActivationDelay = 1.0f;

    CoinMover coinMover;
    BoxCollider2D boxCollider;
    bool collected;

    void Start()
    {
        coinMover = GetComponentInParent<CoinMover>();
        boxCollider = GetComponent<BoxCollider2D>();

        boxCollider.enabled = false;
        coinMover.enabled = false;
        collected = false;
        StartCoroutine(Activator());
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!collected && other.gameObject.layer == Layers.Player)
        {
            collected = true;
            enabled = false;
            GetComponentInParent<Rigidbody2D>().isKinematic = true;
            GetComponentInParent<BoxCollider2D>().enabled = false;
            coinMover.enabled = true; 
        }
    }

    IEnumerator Activator()
    {
        yield return new WaitForSeconds(ActivationDelay);
        boxCollider.enabled = true;
    }
}

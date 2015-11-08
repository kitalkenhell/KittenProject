using UnityEngine;
using System.Collections;

public class DynamicCoinTrigger : MonoBehaviour 
{
    public float ActivationDelay = 1.0f;
    
    CoinMover coinMover;
    BoxCollider2D boxCollider;
    bool collected;
    Rigidbody2D parentBody;
    BoxCollider2D parentCollider;

    public void Awake()
    {
        coinMover = GetComponentInParent<CoinMover>();
        boxCollider = GetComponent<BoxCollider2D>();
        parentBody = GetComponentInParent<Rigidbody2D>();
        parentCollider = GetComponentInParent<BoxCollider2D>();
    }

    public void Reset()
    {
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
            parentBody.isKinematic = true;
            parentCollider.enabled = false;
            coinMover.enabled = true; 
        }
    }

    IEnumerator Activator()
    {
        yield return new WaitForSeconds(ActivationDelay);
        boxCollider.enabled = true;
    }
}

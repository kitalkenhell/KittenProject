using UnityEngine;
using System.Collections;

public class DynamicCoinResetter : MonoBehaviour 
{
    Vector3 coinScale;
    Rigidbody2D body;
    CoinMover mover;
    DynamicCoinTrigger trigger;

    public void Awake()
    {
        coinScale = transform.localScale;
        mover = GetComponent<CoinMover>();
        body = GetComponent<Rigidbody2D>();
        trigger = GetComponentInChildren<DynamicCoinTrigger>();
    }

	public void Reset(Vector3 position, Vector2 velocity, float angularSpeed, Transform target)
    {
        gameObject.SetActive(true);
        mover.target = target;
        transform.position = position;
        transform.localScale = coinScale;
        body.velocity = velocity;
		body.angularVelocity = angularSpeed;
        body.isKinematic = false;
        trigger.Reset();
    }
}

using UnityEngine;
using System.Collections;

public class DynamicCoinResetter : MonoBehaviour 
{
    Vector3 coinScale;
    Rigidbody2D body;
    Pickup mover;
    DynamicCoinTrigger trigger;

    public void Awake()
    {
        coinScale = transform.localScale;
        mover = GetComponent<Pickup>();
        body = GetComponent<Rigidbody2D>();
        trigger = GetComponentInChildren<DynamicCoinTrigger>();
    }

	public void Reset(Vector3 position, Vector2 velocity, float angularSpeed, PlayerLogic player)
    {
        gameObject.SetActive(true);
        mover.player = player;
        transform.position = position;
        transform.localScale = coinScale;
        body.velocity = velocity;
		body.angularVelocity = angularSpeed;
        body.isKinematic = false;
        trigger.Reset();
    }
}

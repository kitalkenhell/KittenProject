using UnityEngine;
using System.Collections;

public class BurningRigidbody : MonoBehaviour 
{
    public Rigidbody2D body;
    public Vector2 offset;

    float collisionCounter = 0;

	void Update () 
    {
        const float maxRotation = 3.0f;
        const float collisionCooldown = 0.1f;

        transform.position = body.position + offset;
        collisionCounter -= Time.deltaTime;

        if (!body.IsTouchingLayers() && collisionCounter < 0)
        {
            transform.up = Vector3.MoveTowards(transform.up, -body.velocity.normalized, maxRotation * Time.deltaTime); 
        }
        else
        {
            const float weightX = 0.2f;
            const float weightY = 0.8f;
            const float randomFactor = 0.3f;

            Vector2 target = new Vector2(Mathf.Sign(-body.velocity.x) * weightX + Random.Range(-randomFactor, randomFactor), weightY).normalized;

            collisionCounter = collisionCooldown;
            transform.up = Vector3.MoveTowards(transform.up, target, maxRotation * Time.deltaTime);
        }
	}
}

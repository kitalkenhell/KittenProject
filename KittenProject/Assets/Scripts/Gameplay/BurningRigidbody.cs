using UnityEngine;
using System.Collections;

public class BurningRigidbody : MonoBehaviour 
{
    public Rigidbody2D body;
    public Vector2 offset;

    float collisionCounter = 0;

    void Start()
    {
        transform.up = -body.velocity.normalized;
    }

	void Update () 
    {
        const float maxRotationFlying = 8.0f;
        const float maxRotationColliding = 3.0f;
        const float collisionCooldown = 0.1f;
        const float velocityThreshold = 20.0f;

        transform.position = body.position + offset;
        collisionCounter -= Time.deltaTime;

        if ((!body.IsTouchingLayers() && collisionCounter < 0) || body.velocity.magnitude > velocityThreshold)
        {
            transform.up = Vector3.MoveTowards(transform.up, -body.velocity.normalized, maxRotationFlying * Time.deltaTime); 
        }
        else
        {
            const float weightX = 0.2f;
            const float weightY = 0.8f;
            const float randomFactor = 0.3f;

            Vector2 target = new Vector2(Mathf.Sign(-body.velocity.x) * weightX + Random.Range(-randomFactor, randomFactor), weightY).normalized;

            collisionCounter = collisionCooldown;
            transform.up = Vector3.MoveTowards(transform.up, target, maxRotationColliding * Time.deltaTime);
        }

        transform.SetPositionZ(transform.parent.position.z);
	}
}

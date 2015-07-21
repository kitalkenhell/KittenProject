using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    const int contactPointIndex = 0;
    const float ignoreCollisionThreshold = 0.01f;

    public float movementSpeed;
    public float acceleration;
    public float friction;
    public float jumpVelocity;
    public float jumpDuration;
    public Vector2 wallBounceVelocity;
    public float wallBounceDuration;

    Rigidbody2D body;

    Vector3 velocity; 
    float jumpingCountdown;
    int groundedCounter;
    int wallSlidingCounter;
    float wallBounceCountdown;
    float wallDirection;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        groundedCounter = wallSlidingCounter = 0;
        jumpingCountdown = wallBounceCountdown = Mathf.NegativeInfinity;
    }

    void FixedUpdate()
    {
        velocity = body.velocity;

        Running();
        Jumping();

        body.velocity = velocity;

        if (Mathf.Abs(body.velocity.x) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = body.velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void Running()
    {
        if (wallBounceCountdown < 0)
        {
            float input = Input.GetAxis("Horizontal");

            velocity.x += input * acceleration;
        
            float sign = Mathf.Sign(velocity.x);

            if (Mathf.Sign(velocity.x) != Mathf.Sign(sign) || Mathf.Abs(input) < Mathf.Epsilon) //don't apply friction when accelerating
            {
                velocity.x -= friction * sign;
                if (Mathf.Sign(velocity.x) != sign)
                {
                    velocity.x = 0;
                }
            }

            velocity.x = Mathf.Clamp(velocity.x, -movementSpeed, movementSpeed);
        }
    }

    void Jumping()
    {
        jumpingCountdown -= Time.fixedDeltaTime;
        wallBounceCountdown -= Time.fixedDeltaTime;

        if (velocity.y > jumpVelocity)
        {
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        if (Input.GetButton("Jump"))
        {
            if (jumpingCountdown < 0) 
            {
                if (groundedCounter > 0) //stands on the ground and jumps 
                {
                    groundedCounter = 0;
                    velocity.y = jumpVelocity;
                    jumpingCountdown = jumpDuration;
                }
            }

            if (jumpingCountdown > 0 && wallSlidingCounter <= 0) //Don't decrease velocity when jump button is pressed for some time the jump 
            {
                velocity.y = jumpVelocity;
            }

            else if (wallSlidingCounter > 0 && groundedCounter <= 0) //bouncing off the wall
            {
                wallBounceCountdown = wallBounceDuration;
                jumpingCountdown = Mathf.NegativeInfinity;
                velocity = new Vector2(wallDirection * wallBounceVelocity.x, wallBounceVelocity.y);
            }
        }
        else
        {
            jumpingCountdown = Mathf.NegativeInfinity;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (collision.gameObject.layer == Layers.Ground)
        {
            if (Vector2.Dot(normal, Vector2.up) < ignoreCollisionThreshold)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                body.velocity = velocity;
            }
            else
            {
                ++groundedCounter;
            }
        }
        else if (collision.gameObject.layer == Layers.Wall)
        {
            wallDirection = Mathf.Sign(normal.x);
            ++wallSlidingCounter;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (collision.gameObject.layer == Layers.Ground)
        {
            if (Vector2.Dot(normal, Vector2.up) > ignoreCollisionThreshold)
            {
                groundedCounter = Mathf.Max(0, --groundedCounter);
            }
        }
        else if (collision.gameObject.layer == Layers.Wall)
        {
            wallSlidingCounter = Mathf.Max(0, --wallSlidingCounter);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}
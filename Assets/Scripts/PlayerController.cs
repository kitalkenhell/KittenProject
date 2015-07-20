using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    const int contactPointIndex = 0;
    const float ignoreCollisionThreshold = 0.01f;

    public float movementSpeed = 20.0f;
    public float jumpVelocity = 30.0f;
    public float jumpDuration = 0.02f;

    Rigidbody2D body;

    Vector3 velocity; 
    float jumpingCountdown;
    int grounded;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        grounded = 0;
        jumpingCountdown = Mathf.NegativeInfinity;
    }

    void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");

        velocity = body.velocity;
        velocity.x = input * movementSpeed;

        Jumping();

        body.velocity = velocity;

        if (Mathf.Abs(body.velocity.x) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = body.velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void Jumping()
    {
        jumpingCountdown -= Time.fixedDeltaTime;

        if (velocity.y > jumpVelocity)
        {
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        if (Input.GetButton("Jump"))
        {
            if (jumpingCountdown < 0)
            {
                if (grounded > 0)
                {
                    grounded = 0;
                    velocity.y = jumpVelocity;
                    jumpingCountdown = jumpDuration;
                }
            }

            if (jumpingCountdown > 0)
            {
                velocity.y = jumpVelocity;
            }
        }
        else
        {
            jumpingCountdown = Mathf.NegativeInfinity;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 normal = collision.contacts[contactPointIndex].normal;

        if (Vector3.Dot(normal, Vector3.up) < ignoreCollisionThreshold)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            body.velocity = velocity;
        }
        else
        {
            ++grounded;
            print(grounded);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Vector3 normal = collision.contacts[contactPointIndex].normal;

        if (Vector3.Dot(normal, Vector3.up) > ignoreCollisionThreshold)
        {
            grounded = Mathf.Max(0, --grounded);
            print(grounded);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}

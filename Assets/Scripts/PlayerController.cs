using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 20.0f;
    public float jumpVelocity = 30.0f;
    public float jumpDuration = 0.02f;

    Rigidbody2D body;

    Transform groundedTestOrigin;
    BoxCollider2D groundedTestOriginExtents;

    Vector3 velocity; 
    float jumpingCountdown;

    static bool blocked = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        groundedTestOrigin = transform.Find("GroundedTestOrigin");
        groundedTestOriginExtents = groundedTestOrigin.GetComponent<BoxCollider2D>();

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
                if (IsGrounded())
                {
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

    bool IsGrounded()
    {
        RaycastHit2D hit;

        hit = Physics2D.BoxCast(new Vector2(groundedTestOrigin.position.x, groundedTestOrigin.position.y),
                                groundedTestOriginExtents.size, 0, Vector2.down, 0, ~(1 << Layers.Player));

        return hit.collider != null;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        const int contactPointIndex = 0;
        const float ignoreCollisionThreshold = 0.01f;

        Vector3 normal = collision.contacts[contactPointIndex].normal;

        if (Vector3.Dot(normal, Vector3.up) < ignoreCollisionThreshold)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
            body.velocity = velocity;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        print("OnTriggerExit2D");
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}

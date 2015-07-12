using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpSpeed = 5;

    Rigidbody2D body;

    Transform groundedTestOrigin;
    BoxCollider2D groundedTestOriginExtents;

    Vector3 v;

    static bool blocked = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        groundedTestOrigin = transform.Find("GroundedTestOrigin");
        groundedTestOriginExtents = groundedTestOrigin.GetComponent<BoxCollider2D>();

        Collider2D col = GameObject.Find("Platform").GetComponent<Collider2D>();

        //Physics2D.IgnoreCollision(col, GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {
        float input = Input.GetAxis("Horizontal");
        Vector2 velocity = body.velocity;

        velocity.x = input * movementSpeed;

        if (Input.GetButton("Jump") && IsGrounded())
        {
            velocity.y = jumpSpeed;
        }

        body.velocity = velocity;

        if (Mathf.Abs(body.velocity.x) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = body.velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        Debug.DrawLine(transform.position, GameObject.Find("Platform").transform.position);


        v = body.velocity;
    }

    bool IsGrounded()
    {
        RaycastHit2D hit;

        hit = Physics2D.BoxCast(new Vector2(groundedTestOrigin.position.x, groundedTestOrigin.position.y),
                                groundedTestOriginExtents.size, 0, Vector2.down, 0, ~(1 << 8));

        return hit.collider != null;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

        //Vector3 myCollisionNormal = collisionInfo.contacts[0].normal;

        Vector3 normal = collision.contacts[0].normal;

        if (Vector3.Dot(normal, Vector3.up) < 0.01f)
        {
            print(normal.ToString() + Vector3.Dot(normal, Vector3.up) + " - IGNORE");
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
        else
        {
            print(normal.ToString() + Vector3.Dot(normal, Vector3.up) + " - BLOCK");
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
        }

    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        print("LEFT");
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}

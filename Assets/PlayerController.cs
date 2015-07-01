using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5;
    public float jumpSpeed = 5;

    Rigidbody2D body;
    Animator animator;

    int speedAnimHash;
    bool grounded;

    Transform groundedTestOrigin;
    BoxCollider2D groundedTestOriginExtents; 

	void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        speedAnimHash = Animator.StringToHash("speed");
        grounded = false;

        groundedTestOrigin = transform.Find("GroundedTestOrigin");
        groundedTestOriginExtents = groundedTestOrigin.GetComponent<BoxCollider2D>();
	}

	void FixedUpdate() 
    {
        float input = Input.GetAxis("Horizontal");
        Vector2 velocity = body.velocity;

        velocity.x = input * movementSpeed;

        animator.SetFloat(speedAnimHash, Mathf.Abs(input));

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
	}

    bool IsGrounded()
    {
        RaycastHit2D hit;
        grounded = true;

        hit = Physics2D.BoxCast(new Vector2(groundedTestOrigin.position.x, groundedTestOrigin.position.y), 
                                groundedTestOriginExtents.size, 0, Vector2.down, 0);

        return hit.collider != null;
    }
}

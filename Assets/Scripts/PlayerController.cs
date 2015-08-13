using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    const int contactPointIndex = 0;
    const float ignoreGroundCollisionThreshold = 0.01f;
    const float ignoreWallCollisionThreshold = 0.99f;
    const string spriteName = "sprite";

    public float movementSpeed;
    public float acceleration;
    public float friction;
    public float jumpSpeed;
    public float jumpDuration;
    public Vector2 wallBounceVelocity;
    public float wallBounceDuration;
    public float wallSlideSpeed;
    public float wallFriction;
    public float propellerFallingSpeed;
    public float propellerDelay; 

    Rigidbody2D body;
    MovablePlatformEffector movablePlatformEffector;
    Transform sprite;

    Vector2 velocity;
    Vector2 preCollisionVelocity;
    float runningMotrSpeed;

    Vector2 movablePlatformVelocity;
    bool onMovablePlatform;

    float relativeJumpSpeed;
    float jumpingCountdown;
    int groundedCounter;

    int wallSlidingCounter;
    float wallBounceCountdown;
    float wallDirection;

    bool usingPropeller;
    float PropellerCountdown;

    float input;
    bool jumpKeyReleased;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        movablePlatformEffector = GetComponent<MovablePlatformEffector>();
        movablePlatformEffector.onMoved = OnMovablePlatformMoved;
        movablePlatformEffector.onEnter = OnMovablePlatformEnter;
        movablePlatformEffector.onExit = OnMovablePlatformExit;

        groundedCounter = 0;
        wallSlidingCounter = 0;
        runningMotrSpeed = 0;
        input = 0;
        relativeJumpSpeed = jumpSpeed;

        jumpingCountdown = Mathf.NegativeInfinity;
        wallBounceCountdown = Mathf.NegativeInfinity;
        PropellerCountdown = Mathf.Infinity;

        onMovablePlatform = false;
        jumpKeyReleased = true;
        usingPropeller = false;

        sprite = transform.Find(spriteName);
    }

    void FixedUpdate()
    {
        Moving();
        Jumping();
        WallSliding();

        body.velocity = velocity;
        preCollisionVelocity = body.velocity;

        if (Mathf.Abs(runningMotrSpeed) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = runningMotrSpeed < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            sprite.transform.localScale = scale;
        }
    }

    void Moving()
    {
        if (wallBounceCountdown < 0)
        {
            input = Input.GetAxis("Horizontal");

            runningMotrSpeed += input * acceleration;

            float sign = Mathf.Sign(runningMotrSpeed);

            if ((Mathf.Sign(input) != Mathf.Sign(sign) || Mathf.Abs(input) < Mathf.Epsilon)  && //don't apply friction when accelerating
                (onMovablePlatform || (!onMovablePlatform && Mathf.Abs(movablePlatformVelocity.x) < Mathf.Epsilon)))  
            {
                runningMotrSpeed -= friction * sign;
                if (Mathf.Sign(runningMotrSpeed) != sign)
                {
                    runningMotrSpeed = 0;
                }
            }
            runningMotrSpeed = Mathf.Clamp(runningMotrSpeed, -movementSpeed, movementSpeed);
        }

        if (onMovablePlatform && movablePlatformVelocity.y < 0)
        {
            velocity.y += movablePlatformVelocity.y;
        }
        else if (!onMovablePlatform && Mathf.Abs(movablePlatformVelocity.x) > Mathf.Epsilon)
        {
            float sign = Mathf.Sign(movablePlatformVelocity.x);

            movablePlatformVelocity.x -= friction * sign;
            if (Mathf.Sign(movablePlatformVelocity.x) != sign)
            {
                movablePlatformVelocity.x = 0;
            }
        }

        velocity.x = runningMotrSpeed + movablePlatformVelocity.x;
    }

    void Jumping()
    {
        
        jumpingCountdown -= Time.fixedDeltaTime;
        wallBounceCountdown -= Time.fixedDeltaTime;
        PropellerCountdown -= Time.fixedDeltaTime;

        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        if (velocity.y > relativeJumpSpeed)
        {
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        if (Input.GetButton("Jump"))
        {
            if (jumpingCountdown < 0) 
            {
                if (groundedCounter > 0 && jumpKeyReleased) //stands on the ground and jumps 
                {
                    jumpKeyReleased = false;
                    relativeJumpSpeed = jumpSpeed + movablePlatformVelocity.y;
                    groundedCounter = 0;
                    velocity.y = relativeJumpSpeed;
                    jumpingCountdown = jumpDuration;
                }
            }

            if (!usingPropeller && groundedCounter <= 0 && wallSlidingCounter <= 0 && jumpingCountdown < 0 && velocity.y < 0 && jumpKeyReleased)
            {
                usingPropeller = true;
                PropellerCountdown = propellerDelay;
            }

            if (usingPropeller && PropellerCountdown < 0 && velocity.y < propellerFallingSpeed)
            {
                jumpKeyReleased = false;
                velocity.y = propellerFallingSpeed;
            }

            if (jumpingCountdown > 0 && wallSlidingCounter <= 0) //Don't decrease velocity when jump button is pressed for some time after the jump 
            {
                velocity.y = relativeJumpSpeed;
            }

            else if (wallSlidingCounter > 0 && groundedCounter <= 0 && jumpKeyReleased) //bouncing off the wall
            {
                jumpKeyReleased = false;
                wallSlidingCounter = 0;
                wallBounceCountdown = wallBounceDuration;
                jumpingCountdown = Mathf.NegativeInfinity;
                runningMotrSpeed = wallDirection * wallBounceVelocity.x;
                velocity = new Vector2(runningMotrSpeed, wallBounceVelocity.y);
            }
        }
        else
        {
            usingPropeller = false;
            jumpKeyReleased = true;
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
            PropellerCountdown = Mathf.NegativeInfinity;
        }
    }

    void WallSliding()
    {
        if (velocity.y < 0 && wallSlidingCounter > 0 && Mathf.Abs(input) > Mathf.Epsilon && Mathf.Sign(input) != wallDirection)
        {
            if (velocity.y < wallSlideSpeed)
            {
                velocity.y += wallFriction;
                velocity.y = Mathf.Min(velocity.y, wallSlideSpeed);
            }
        }
    }

    public void OnMovablePlatformMoved(Vector3 displacement)
    {
        movablePlatformVelocity = new Vector2(displacement.x, displacement.y) / Time.deltaTime;
    }

    void OnMovablePlatformEnter()
    {
        onMovablePlatform = true;
    }

    void OnMovablePlatformExit()
    {
        onMovablePlatform = false;
        movablePlatformVelocity.y = 0;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (collision.gameObject.layer == Layers.Ground)
        {
            if (Vector2.Dot(normal, Vector2.up) < ignoreGroundCollisionThreshold)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                body.velocity = preCollisionVelocity;
            }
            else
            {
                usingPropeller = false;
                ++groundedCounter;

                if (velocity.y < 0)
                {
                    velocity.y = 0;
                }

            }
        }
        else if (collision.gameObject.layer == Layers.Wall)
        {
            wallDirection = Mathf.Sign(normal.x);

            if (Vector2.Dot(normal, Vector2.right * wallDirection) > ignoreWallCollisionThreshold)
            {
                ++wallSlidingCounter;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if ((collision.gameObject.layer == Layers.Ground && Vector2.Dot(normal, Vector2.up) > ignoreGroundCollisionThreshold) ||
            (collision.gameObject.layer == Layers.Wall && Vector2.Dot(normal, Vector2.right * wallDirection) < ignoreWallCollisionThreshold) &&
            (velocity.y < 0))
        {
            velocity.y = 0;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (collision.gameObject.layer == Layers.Ground)
        {
            if (Vector2.Dot(normal, Vector2.up) > ignoreGroundCollisionThreshold)
            {
                groundedCounter = Mathf.Max(0, --groundedCounter);
            }
        }
        else if (collision.gameObject.layer == Layers.Wall)
        {
            if (Vector2.Dot(normal, Vector2.right * wallDirection) > ignoreWallCollisionThreshold)
            {
                wallSlidingCounter = Mathf.Max(0, --wallSlidingCounter);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}
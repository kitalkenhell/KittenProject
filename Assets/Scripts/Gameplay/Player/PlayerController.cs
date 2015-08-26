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
    public float jumpSpeedMin;
    public float jumpSpeedMax;
    public float jumpDuration;
    public Vector2 wallBounceVelocity;
    public float wallBounceDuration;
    public float wallSlideSpeed;
    public float wallFriction;
    public float propellerFallingSpeed;
    public float propellerDelay;
    public InputManager inputManager;

    Rigidbody2D body;
    MovablePlatformEffector movablePlatformEffector;
    Transform sprite;

    Vector2 velocity;
    Vector2 preCollisionVelocity;
    float runningMotrSpeed;

    Vector2 movablePlatformVelocity;
    bool onMovablePlatform;

    float jumpSpeed;
    float relativeJumpSpeed;
    float jumpingCountdown;
    int groundedCounter;

    int wallSlidingCounter;
    float wallDirection;

    bool usingPropeller;
    float PropellerCountdown;

    float pushingForce;

    float input;
    bool jumpKeyReleased;
    float disableControlsCountdown;

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
        relativeJumpSpeed = jumpSpeed;

        jumpingCountdown = Mathf.NegativeInfinity;
        disableControlsCountdown = Mathf.NegativeInfinity;
        PropellerCountdown = Mathf.Infinity;

        onMovablePlatform = false;
        jumpKeyReleased = true;
        usingPropeller = false;

        input = 0;
        jumpSpeed = jumpSpeedMin;

        sprite = transform.Find(spriteName);
    }

    void FixedUpdate()
    {
        disableControlsCountdown -= Time.fixedDeltaTime;

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

        PostOffice.PostDebugMessage(velocity.ToString());
    }

    void Moving()
    {
        if (disableControlsCountdown < 0)
        {
            input = inputManager.horizontalAxis;

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

        else if (Mathf.Abs(pushingForce) > Mathf.Epsilon && disableControlsCountdown < 0)
        {
            float sign = Mathf.Sign(pushingForce);

            pushingForce -= friction * sign;
            if (Mathf.Sign(pushingForce) != sign)
            {
                pushingForce = 0;
            }
        }

        velocity.x = runningMotrSpeed + movablePlatformVelocity.x + pushingForce;
    }

    void Jumping()
    {
        
        jumpingCountdown -= Time.fixedDeltaTime;
        PropellerCountdown -= Time.fixedDeltaTime;

        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        if (velocity.y > relativeJumpSpeed)
        {
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        if (inputManager.jumpButtonDown)
        {
            if (jumpingCountdown < 0) 
            {
                if (groundedCounter > 0 && jumpKeyReleased) //stands on the ground and jumps 
                {
                    jumpSpeed = jumpSpeedMin + Mathf.Clamp01(Mathf.Abs(velocity.x) / movementSpeed) * (jumpSpeedMax - jumpSpeedMin);

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
                disableControlsCountdown = wallBounceDuration;
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

    public void Push(Vector2 force, bool overrideVelocityX = false, bool overrideVelocityY = true, float disableControlsDuration = 0.0f)
    {
        pushingForce = force.x;

        if (overrideVelocityX)
        {
            runningMotrSpeed = 0;
        }

        velocity.y = overrideVelocityY ? force.y : velocity.y + force.y;

        disableControlsCountdown = disableControlsDuration;
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
        pushingForce += movablePlatformVelocity.x;
        movablePlatformVelocity = Vector2.zero;
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

        if (((collision.gameObject.layer == Layers.Ground && Vector2.Dot(normal, Vector2.up) > ignoreGroundCollisionThreshold) ||
            (collision.gameObject.layer == Layers.Wall && Vector2.Dot(normal, Vector2.right * wallDirection) < ignoreWallCollisionThreshold)) &&
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
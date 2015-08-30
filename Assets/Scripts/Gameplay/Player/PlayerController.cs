using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    const string spriteName = "sprite";

    Vector2 tmp;
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
    public LayerMask onewayMask;
    public LayerMask obstaclesMask;
    public InputManager inputManager;

    Rigidbody2D body;
    BoxCollider2D boxCollider;
    MovablePlatformEffector movablePlatformEffector;
    CoinEmitter coinEmitter;
    Transform sprite;

    Vector2 velocity;
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

    int coins;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        movablePlatformEffector = GetComponent<MovablePlatformEffector>();
        coinEmitter = GetComponent<CoinEmitter>();

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

        coins = 0;

        PostOffice.coinCollected += OnCoinCollected;
    }

    void OnDestroy()
    {
        PostOffice.coinCollected -= OnCoinCollected;
    }

    void Update()
    {
        disableControlsCountdown -= Time.deltaTime;

        Movement();
        Jumping();
        WallSliding();
        Move();

        if (Mathf.Abs(runningMotrSpeed) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = runningMotrSpeed < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            sprite.transform.localScale = scale;
        }
    }

    void Movement()
    {
        if (disableControlsCountdown < 0)
        {
            input = inputManager.horizontalAxis;

            runningMotrSpeed += input * acceleration;

            float sign = Mathf.Sign(runningMotrSpeed);

            if ((Mathf.Sign(input) != Mathf.Sign(sign) || Mathf.Abs(input) < Mathf.Epsilon) && //don't apply friction when accelerating
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

        jumpingCountdown -= Time.deltaTime;
        PropellerCountdown -= Time.deltaTime;

        velocity.y += Physics2D.gravity.y * Time.deltaTime;

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

    public void Hit(int damage)
    {
        coinEmitter.Emit(coins);
        coins = 0;
    }

    void OnCoinCollected(int amount)
    {
        coins += amount;
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

    /*public void OnColdlisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (collision.gameObject.layer == Layers.Ground)
        {
            PostOffice.PostDebugMessage(Vector2.Dot(normal, Vector2.up).ToString());
            if (Vector2.Dot(normal, Vector2.up) < ignoreGroundCollisionThreshold)
            {
                //PostOffice.PostDebugMessage("ignore");
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
                //     body.velocity = velocity;
            }
            else
            {

                //PostOffice.PostDebugMessage("reset");
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


    public void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
    }

    public void OnCollisfionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[contactPointIndex].normal;

        if (((collision.gameObject.layer == Layers.Ground && Vector2.Dot(normal, Vector2.up) > ignoreGroundCollisionThreshold) ||
            (collision.gameObject.layer == Layers.Wall && Vector2.Dot(normal, Vector2.right * wallDirection) < ignoreWallCollisionThreshold)) &&
            (velocity.y < 0))
        {
            velocity.y = 0;
        }
    }

    public void OnCollisfionExit2D(Collision2D collision)
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
    */

    public void Move()
    {
        const float bias = 1.2f;

        Vector2 displacement = velocity * Time.deltaTime;
        RaycastHit2D hit;

        groundedCounter = 0;
        wallSlidingCounter = 0;

        if (velocity.y < Mathf.Epsilon)
        {
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 
                Mathf.Max(Mathf.Abs(displacement.y),bias), obstaclesMask | onewayMask);
            if (hit.collider != null && hit.collider.bounds.max.y < boxCollider.bounds.min.y)
            {
                displacement.y = hit.collider.bounds.max.y - boxCollider.bounds.min.y;
                velocity.y = 0;
                groundedCounter = 1;
                print(displacement.y);
                //Debug.Break();
            }
        }
        else if (velocity.y > Mathf.Epsilon)
        {
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.up, Mathf.Abs(displacement.y), obstaclesMask);
            if (hit.collider != null && hit.collider.bounds.min.y > boxCollider.bounds.max.y - bias)
            {
                displacement.y = hit.collider.bounds.min.y - boxCollider.bounds.max.y - bias;
                velocity.y = 0;
            }
        }
        
        hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.right * Mathf.Sign(displacement.x), Mathf.Abs(displacement.x), obstaclesMask);
        if (hit.collider != null)
        {
            if (velocity.x < 0 && hit.collider.bounds.max.x < boxCollider.bounds.min.x + bias)
            {
                displacement.x = hit.collider.bounds.max.x - boxCollider.bounds.min.x + bias; 
            }
            else if (velocity.x > 0 && hit.collider.bounds.min.x > boxCollider.bounds.max.x - bias)
            {
                displacement.x = hit.collider.bounds.min.x - boxCollider.bounds.max.x - bias;
            }
            wallDirection = -Mathf.Sign(velocity.x);
            wallSlidingCounter = 1;
            velocity.x = 0;
        }

        print("V = " + velocity.y);

        if (Mathf.Abs(velocity.y) > Mathf.Epsilon)
        {
            print("");
            transform.SetPositionY(transform.position.y + displacement.y);
        }
        if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
        {
            transform.SetPositionX(transform.position.x + displacement.x);
        }
    }
}
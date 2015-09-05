using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Vector2 tmp;
    public float movementSpeed;
    public float acceleration;
    public float friction;
    public float jumpSpeedMin;
    public float jumpSpeedMax;
    public float jumpDuration;
    public float movablePlatformJumpSpeedInfluence;
    public Vector2 wallBounceVelocity;
    public float wallBounceDuration;
    public float wallSlideSpeed;
    public float wallFriction;
    public float parachuteFallingSpeed;
    public float parachuteDelay;
    public float parachuteMaxScale;
    public float parachuteMaxRotation;
    public float parachuteOpeningSpeed;
    public float parachuteClosingSpeed;
    public AnimationCurve parachuteOpeningCurve;
    public int coinsDropRate;
    public LayerMask onewayMask;
    public LayerMask obstaclesMask;
    public InputManager inputManager;
    public Transform sprite;
    public Transform parachute;
    public Transform parachutePivot;

    BoxCollider2D boxCollider;
    CoinEmitter coinEmitter;

    Vector2 velocity;
    float runningMotrSpeed;

    Vector2 movablePlatformVelocity;
    MovablePlatform movablePlatform;
    BoxCollider2D movablePlatformCollider;

    float jumpSpeed;
    float relativeJumpSpeed;
    float jumpingCountdown;
    bool isGrounded;

    bool isTouchingWall;
    float wallDirection;

    bool usingParachute;
    float ParachuteCountdown;
    float parachuteScale;
    float parachuteRotation;

    float pushingForce;

    float input;
    bool jumpKeyReleased;
    float disableControlsCountdown;
    

    int coins;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        coinEmitter = GetComponent<CoinEmitter>();

        isGrounded = false;
        isTouchingWall = false;
        runningMotrSpeed = 0;
        relativeJumpSpeed = jumpSpeed;

        jumpingCountdown = Mathf.NegativeInfinity;
        disableControlsCountdown = Mathf.NegativeInfinity;
        ParachuteCountdown = Mathf.Infinity;

        jumpKeyReleased = true;
        usingParachute = false;
        parachuteScale = 0;

        input = 0;
        jumpSpeed = jumpSpeedMin;

        movablePlatform = null;

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
        UpdateSprites();
    }

    void UpdateSprites()
    {
        if (Mathf.Abs(runningMotrSpeed) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = runningMotrSpeed + pushingForce < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            sprite.transform.localScale = scale;
        }

        parachute.localScale = Vector3.one * parachuteOpeningCurve.Evaluate(parachuteScale) * parachuteMaxScale;

        if (!usingParachute)
        {
            parachuteRotation = Mathf.MoveTowardsAngle(parachuteRotation, 0, parachuteClosingSpeed * Time.fixedDeltaTime);
        }
        else
        {
            parachuteRotation = Mathf.MoveTowardsAngle(parachuteRotation, -input * parachuteMaxRotation, parachuteOpeningSpeed * Time.fixedDeltaTime);
        }

        parachutePivot.rotation = Quaternion.Euler(0, 0, parachuteRotation);
    }

    void Movement()
    {
        if (disableControlsCountdown < 0)
        {
            input = inputManager.horizontalAxis;

            runningMotrSpeed += input * acceleration;

            float sign = Mathf.Sign(runningMotrSpeed);

            if (Mathf.Sign(input) != Mathf.Sign(sign) || Mathf.Abs(input) < Mathf.Epsilon)
            {
                runningMotrSpeed -= friction * sign;
                if (Mathf.Sign(runningMotrSpeed) != sign)
                {
                    runningMotrSpeed = 0;
                }
            }
            runningMotrSpeed = Mathf.Clamp(runningMotrSpeed, -movementSpeed, movementSpeed);
        }

        if (Mathf.Abs(pushingForce) > Mathf.Epsilon && disableControlsCountdown < 0)
        {
            float sign = Mathf.Sign(pushingForce);

            pushingForce -= friction * sign;
            if (Mathf.Sign(pushingForce) != sign)
            {
                pushingForce = 0;
            }
        }

        velocity.x = runningMotrSpeed + pushingForce;
    }

    void Jumping()
    {

        jumpingCountdown -= Time.deltaTime;
        ParachuteCountdown -= Time.deltaTime;

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
                if (isGrounded && jumpKeyReleased) //stands on the ground and jumps 
                {
                    jumpSpeed = jumpSpeedMin + Mathf.Clamp01(Mathf.Abs(velocity.x) / movementSpeed) * (jumpSpeedMax - jumpSpeedMin);

                    jumpKeyReleased = false;
                    relativeJumpSpeed = jumpSpeed + movablePlatformVelocity.y * movablePlatformJumpSpeedInfluence;
                    isGrounded = false;
                    velocity.y = relativeJumpSpeed;
                    jumpingCountdown = jumpDuration;
                }
            }

            if (velocity.y >= 0)
            {
                usingParachute = false;
            }
            else if (!usingParachute && !isGrounded && !isTouchingWall && jumpingCountdown < 0 && jumpKeyReleased)
            {
                usingParachute = true;
                ParachuteCountdown = parachuteDelay;
            }


            if (usingParachute && ParachuteCountdown < 0 && velocity.y < parachuteFallingSpeed)
            {
                jumpKeyReleased = false;
                velocity.y = parachuteFallingSpeed;
            }

            if (jumpingCountdown > 0 && !isTouchingWall) //Don't decrease velocity when jump button is pressed for some time after the jump 
            {
                velocity.y = relativeJumpSpeed;
            }

            else if (isTouchingWall && !isGrounded  && jumpKeyReleased) //bouncing off the wall
            {
                jumpKeyReleased = false;
                isTouchingWall = false;
                disableControlsCountdown = wallBounceDuration;
                jumpingCountdown = Mathf.NegativeInfinity;
                runningMotrSpeed = wallDirection * wallBounceVelocity.x;
                velocity = new Vector2(runningMotrSpeed, wallBounceVelocity.y);
            }
        }
        else
        {
            usingParachute = false;
            jumpKeyReleased = true;
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        parachuteScale += (usingParachute ? Time.fixedDeltaTime : -Time.fixedDeltaTime) / parachuteDelay;

        parachuteScale = Mathf.Clamp01(parachuteScale);
    }

    void WallSliding()
    {
        if (velocity.y < 0 && isTouchingWall && Mathf.Abs(input) > Mathf.Epsilon && Mathf.Sign(input) != wallDirection)
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
        int drop = Mathf.Min(coins, coinsDropRate);
        coins -= drop;

        coinEmitter.Emit(drop);
    }

    void OnCoinCollected(int amount)
    {
        coins += amount;
    }

    public void Move()
    {
        const float bias = 0.1f;

        Vector2 displacement = velocity * Time.deltaTime;
        RaycastHit2D hit;

        isGrounded = false;
        isTouchingWall = false;
        movablePlatformVelocity = Vector2.zero;

        if (velocity.y > 0 || (movablePlatformCollider != null && (boxCollider.bounds.max.x < movablePlatformCollider.bounds.min.x || boxCollider.bounds.min.x > movablePlatformCollider.bounds.max.x)))
        {
            movablePlatform = null;
        }

        if (movablePlatform != null && velocity.y <= 0)
        {
            velocity.y = displacement.y = 0;
            isGrounded = true;
            usingParachute = false;
            transform.SetPositionXy(transform.position.x + movablePlatform.LastFrameDisplacement.x, transform.position.y + movablePlatform.LastFrameDisplacement.y);
            movablePlatformVelocity = movablePlatform.LastFrameDisplacement / Time.deltaTime;
        }
        else 
        {
            float rayLength = (velocity.y < 0) ? Mathf.Max(Mathf.Abs(displacement.y), bias) : bias;

            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, rayLength, obstaclesMask | onewayMask);
            
            if (hit.collider != null && hit.collider.gameObject.layer == Layers.MovablePlatform)
            {
                movablePlatform = hit.collider.GetComponent<MovablePlatform>();
                movablePlatformVelocity = movablePlatform.LastFrameDisplacement / Time.deltaTime;

                if (hit.collider.bounds.max.y - movablePlatform.LastFrameDisplacement.y < boxCollider.bounds.min.y + movablePlatform.LastFrameDisplacement.y && (velocity.y <= 0 || movablePlatformVelocity.y > velocity.y))
                {
                    movablePlatformCollider = hit.collider.GetComponent<BoxCollider2D>();

                    velocity.y = displacement.y = 0;
                    isGrounded = true;
                    usingParachute = false;
                    transform.SetPositionX(transform.position.x + movablePlatform.LastFrameDisplacement.x);
                    transform.SetPositionY(transform.position.y + hit.collider.bounds.max.y - boxCollider.bounds.min.y + bias);
                }
                else
                {
                    movablePlatform = null;
                }
            }
            else
            {
                movablePlatform = null;
            }
        }

        if (velocity.y < Mathf.Epsilon && movablePlatform == null)
        {
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, Mathf.Max(Mathf.Abs(displacement.y),bias), obstaclesMask | onewayMask);
            if (hit.collider != null && hit.collider.bounds.max.y < boxCollider.bounds.min.y)
            {
                displacement.y = hit.collider.bounds.max.y - boxCollider.bounds.min.y + bias;
                velocity.y = 0;
                isGrounded = true;
                usingParachute = false;
            }
        }
        else if (velocity.y > Mathf.Epsilon)
        {
            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.up, Mathf.Max(Mathf.Abs(displacement.y), bias), obstaclesMask);
            if (hit.collider != null && hit.collider.bounds.min.y > boxCollider.bounds.max.y)
            {
                displacement.y = hit.collider.bounds.min.y - boxCollider.bounds.max.y - bias;
                velocity.y = 0;
            }
        }

        hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.right * Mathf.Sign(displacement.x), Mathf.Max(Mathf.Abs(displacement.x), bias), obstaclesMask);
        if (hit.collider != null)
        {
            if (velocity.x < 0 && hit.collider.bounds.max.x < boxCollider.bounds.min.x)
            {
                displacement.x = hit.collider.bounds.max.x - boxCollider.bounds.min.x + bias; 
            }
            else if (velocity.x > 0 && hit.collider.bounds.min.x > boxCollider.bounds.max.x)
            {
                displacement.x = hit.collider.bounds.min.x - boxCollider.bounds.max.x - bias;
            }
            wallDirection = -Mathf.Sign(velocity.x);
            isTouchingWall = true;
            velocity.x = 0;
        }
        transform.SetPositionXy(transform.position.x + displacement.x, transform.position.y + displacement.y);
    }
}
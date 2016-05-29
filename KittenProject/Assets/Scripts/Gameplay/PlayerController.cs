using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float acceleration;
    public float friction;
    public float slopeLimit;
    public float jumpSpeedMin;
    public float jumpSpeedMax;
    public float jumpDuration;
    public float doubleJumpSpeed;
    public float notGroundedDelay;
    public float movablePlatformJumpSpeedInfluence;
    public Vector2 wallBounceVelocity;
    public float wallBounceDuration;
    public float wallSlideSpeed;
    public float wallFriction;
    public float parachuteFallingSpeed;
    public float parachuteDelay;
    public float parachuteMaxScale;
    public float parachuteMaxRotation;
    public float parachuteOpenRotateSpeed;
    public float parachuteClosedRotateSpeed;
    public float parachuteClosingSpeedFactor;
    public AnimationCurve parachuteOpeningCurve;
    public float parachuteRandomRotationAmplitude;
    public float parachuteRandomRotationFrequency;
    public AnimationCurve parachuteRandomRotationCurve;
    public LayerMask onewayMask;
    public LayerMask obstaclesMask;
    public LayerMask movablePlatformMask;
    public InputManager inputManager;
    public Transform sprite;
    public Transform parachute;
    public Transform parachutePivot;

    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }

    BoxCollider2D boxCollider;
    Animator animator;
    PlayerLogic playerLogic;

    Vector2 preUpdatePosition;

    Vector2 velocity;
    Vector2 HorizontalMovmentDirection;
    float runningMotrSpeed;

    Vector2 movablePlatformVelocity;
    MoveAlongWaypoints movablePlatform;
    Collider2D movablePlatformCollider;

    float jumpSpeed;
    float relativeJumpSpeed;
    float jumpingCountdown;
    float notGroundedCountdown;
    bool isGrounded;
    bool doubleJump;

    bool isTouchingWall;
    float wallDirection;

    bool usingParachute;
    bool inWindZone;
    float windZoneAcceleration;
    float windZoneMaxSpeed;
    float windZoneHeightLimit;
    float ParachuteCountdown;
    float parachuteScale;
    float parachuteRotation;
    float parachuteRandomRotationOffset;
    float usingParachuteTimer;

    int speedAnimHash;
    int jumpAnimHash;
    int doubleJumpAnimHash;
    int fallingSpeedAnimHash;
    int isGroundedAnimHash;
    int wallSlidingAnimHash;

    float pushingForce;

    float input;
    bool jumpKeyReleased;
    float disableControlsCountdown;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = sprite.GetComponent<Animator>();
        playerLogic = GetComponent<PlayerLogic>();

        isGrounded = false;
        isTouchingWall = false;
        doubleJump = false;
        notGroundedCountdown = 0;
        runningMotrSpeed = 0;
        relativeJumpSpeed = jumpSpeed;
        HorizontalMovmentDirection = Vector2.right * Mathf.Sign(velocity.x);

        jumpingCountdown = Mathf.NegativeInfinity;
        disableControlsCountdown = Mathf.NegativeInfinity;
        ParachuteCountdown = Mathf.Infinity;

        jumpKeyReleased = true;
        usingParachute = false;
        parachuteScale = 0;

        windZoneAcceleration = 0;
        windZoneMaxSpeed = 0;

        input = 0;
        jumpSpeed = jumpSpeedMin;

        movablePlatform = null;

        speedAnimHash = Animator.StringToHash("Speed");
        jumpAnimHash = Animator.StringToHash("Jump");
        doubleJumpAnimHash = Animator.StringToHash("DoubleJump");
        fallingSpeedAnimHash = Animator.StringToHash("FallingSpeed");
        isGroundedAnimHash = Animator.StringToHash("IsGrounded");
        wallSlidingAnimHash = Animator.StringToHash("IsWallSliding");
    }

    void Update()
    {
        disableControlsCountdown -= Time.deltaTime;
        preUpdatePosition = transform.position;

        Movement();
        Jumping();
        Move();
        //WallSliding();
        UnstuckSafeguard();
        UpdateSprites();
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        animator.SetFloat(speedAnimHash, Mathf.Abs(velocity.x));
        animator.SetFloat(fallingSpeedAnimHash, velocity.y);
        animator.SetBool(isGroundedAnimHash, isGrounded);
    }

    void UpdateSprites()
    {

        if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
        {
            Vector3 scale = transform.localScale;
            scale.x = velocity.x < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            sprite.transform.localScale = scale; 
        }

        parachuteScale += (usingParachute ? Time.deltaTime : -Time.deltaTime * parachuteClosingSpeedFactor) / parachuteDelay;
        parachuteScale = Mathf.Clamp01(parachuteScale);
        parachute.localScale = Vector3.one * parachuteOpeningCurve.Evaluate(parachuteScale) * parachuteMaxScale;

        if (!usingParachute)
        {
            usingParachuteTimer = 0;
            parachuteRotation = Mathf.MoveTowardsAngle(parachuteRotation, 0, parachuteClosedRotateSpeed * Time.deltaTime);
        }
        else
        {
            if (Mathf.Approximately(input, 0))
            {
                const float phaseShift = 0.5f;

                parachuteRandomRotationOffset = Mathf.PingPong(usingParachuteTimer * parachuteRandomRotationFrequency + phaseShift, 1.0f);
                parachuteRandomRotationOffset *= parachuteRandomRotationCurve.Evaluate(parachuteRandomRotationOffset);
                parachuteRandomRotationOffset = parachuteRandomRotationOffset * 2 - 1; //scale form 0 - 1 to -1 - 1
                parachuteRandomRotationOffset *= parachuteRandomRotationAmplitude * Mathf.Clamp01(usingParachuteTimer);

                parachuteRotation = Mathf.MoveTowardsAngle(parachuteRotation, -input * parachuteMaxRotation + parachuteRandomRotationOffset, parachuteOpenRotateSpeed * Time.deltaTime);

                usingParachuteTimer += Time.deltaTime;
            }
            else
            {
                usingParachuteTimer = 0;
                parachuteRotation = Mathf.MoveTowardsAngle(parachuteRotation, -input * parachuteMaxRotation, parachuteOpenRotateSpeed * Time.deltaTime);
            }

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

        if (!IsGrounded)
        {
            notGroundedCountdown += Time.deltaTime;
        }

        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        if (velocity.y > relativeJumpSpeed)
        {
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
        }

        if (!inputManager.jumpButtonDown)
        {
            usingParachute = false;
            jumpKeyReleased = true;
            relativeJumpSpeed = jumpSpeed;
            jumpingCountdown = Mathf.NegativeInfinity;
        }
        else
        {
            if (jumpingCountdown < 0) //stands on the ground and jumps 
            {
                if (jumpKeyReleased)
                {
                    if (isGrounded || notGroundedCountdown < notGroundedDelay && velocity.y < 0)
                    {
                        jumpSpeed = jumpSpeedMin + Mathf.Clamp01(Mathf.Abs(velocity.x) / movementSpeed) * (jumpSpeedMax - jumpSpeedMin);

                        jumpKeyReleased = false;
                        relativeJumpSpeed = jumpSpeed + movablePlatformVelocity.y * movablePlatformJumpSpeedInfluence;
                        isGrounded = false;
                        doubleJump = false;
                        velocity.y = relativeJumpSpeed;
                        jumpingCountdown = jumpDuration;
                        animator.SetTrigger(jumpAnimHash);
                        return; 
                    }
                }
            }
            else if (!isTouchingWall) //Don't decrease velocity when jump button is pressed for some time after the jump 
            {
                velocity.y = relativeJumpSpeed;
                return;
            }

            if (isTouchingWall && !isGrounded && jumpKeyReleased) //bouncing off the wall
            {
                float wallJumpBias = 0.3f;

                jumpKeyReleased = false;
                isTouchingWall = false;
                doubleJump = false;
                disableControlsCountdown = wallBounceDuration;
                jumpingCountdown = Mathf.NegativeInfinity;
                runningMotrSpeed = wallDirection * wallBounceVelocity.x;
                transform.SetPositionX(transform.position.x + wallDirection * wallJumpBias);
                velocity = new Vector2(runningMotrSpeed, wallBounceVelocity.y);
                animator.SetTrigger(doubleJumpAnimHash);
                return;
            }

            if (!isGrounded && !isTouchingWall && jumpingCountdown < 0 && jumpKeyReleased) //Parachute / double jump
            {
                if (!doubleJump && velocity.y < doubleJumpSpeed)
                {
                    jumpKeyReleased = false;
                    isGrounded = false;
                    velocity.y = doubleJumpSpeed;
                    doubleJump = true;
                    animator.SetTrigger(doubleJumpAnimHash);
                }
                else if (!usingParachute && velocity.y < 0)
                {
                    doubleJump = true;
                    usingParachute = true;
                    ParachuteCountdown = parachuteDelay;
                    jumpKeyReleased = false;
                }
            }
            else if (usingParachute && ParachuteCountdown < 0)
            {
                jumpKeyReleased = false;

                if (inWindZone)
                {
                    if (transform.position.y > windZoneHeightLimit && velocity.y < 0)
                    {
                        velocity.y = Mathf.MoveTowards(velocity.y, 0, Time.deltaTime * windZoneAcceleration);
                    }

                    else if (velocity.y < windZoneMaxSpeed)
                    {
                        velocity.y = Mathf.MoveTowards(velocity.y, windZoneMaxSpeed, Time.deltaTime * windZoneAcceleration);
                    }
                }
                else
                {
                    if (velocity.y < parachuteFallingSpeed)
                    {
                        velocity.y = parachuteFallingSpeed;
                    }
                }
            }
        }
    }

    void WallSliding()
    {
        if (velocity.y < 0 && isTouchingWall && Mathf.Abs(input) > Mathf.Epsilon && Mathf.Sign(input) != wallDirection)
        {
            if (velocity.y < wallSlideSpeed)
            {
                velocity.y += wallFriction;
                velocity.y = Mathf.Min(velocity.y, wallSlideSpeed);
                animator.SetBool(wallSlidingAnimHash, true);
            }
            else
            {
                animator.SetBool(wallSlidingAnimHash, false);
            }
        }
        else
        {
            animator.SetBool(wallSlidingAnimHash, false);
        }
    }

    public void EnterWindZone(float windSpeed, float windAcceleration, float windHeightLimit)
    {
        inWindZone = true;
        windZoneAcceleration = windAcceleration;
        windZoneMaxSpeed = windSpeed;
        windZoneHeightLimit = windHeightLimit;
    }

    public void ExitWindZone()
    {
        inWindZone = false;
        windZoneAcceleration = 0;
        windZoneMaxSpeed = 0;
    }

    public void Push(Vector2 force, bool overrideVelocityX = false, bool overrideVelocityY = true, float disableControlsDuration = 0.0f, bool resetDoubleJump = true)
    {
        pushingForce = force.x;
        usingParachute = false;

        if (overrideVelocityX)
        {
            runningMotrSpeed = 0;
        }

        if (resetDoubleJump)
        {
            doubleJump = false;
        }

        velocity.y = overrideVelocityY ? force.y : velocity.y + force.y;

        disableControlsCountdown = disableControlsDuration;
    }

    public void PushAndHit(Vector2 force, bool overrideVelocityX = false, bool overrideVelocityY = true, float disableControlsDuration = 0.0f, bool resetDoubleJump = true, int damage = 1)
    {
        Push(force, overrideVelocityX, overrideVelocityY, disableControlsDuration, resetDoubleJump);
        playerLogic.DealDamage(damage);
    }

    void OnGrounded(bool resetVelocityY = true)
    {
        if (resetVelocityY)
        {
            velocity.y = 0; 
        }

        isGrounded = true;
        doubleJump = false;
        usingParachute = false;
        notGroundedCountdown = 0;
    }

    public void Move()
    {
        const float bias = 0.25f;
        const float headBias = 0.6f;
        const float minGravityRayLenght = 0.3f;

        //vertical box cast collider
        const float colliderHeight = 0.6f;
        const float halfColliderHeight = colliderHeight / 2.0f;
        Vector2 boxSize = new Vector2(boxCollider.size.x, colliderHeight);
        Vector2 boxOrigin;

        Vector2 displacement = velocity * Time.deltaTime;
        RaycastHit2D hit;

        isGrounded = false;
        isTouchingWall = false;
        movablePlatformVelocity = Vector2.zero;
        HorizontalMovmentDirection = Vector2.right;

        //Movable Platform
        if (velocity.y > 0 || (movablePlatformCollider != null &&
            (boxCollider.bounds.max.x < movablePlatformCollider.bounds.min.x || boxCollider.bounds.min.x > movablePlatformCollider.bounds.max.x)))
        {
            movablePlatform = null;
        }

        if (movablePlatform != null && velocity.y <= 0)
        {
            OnGrounded();
            displacement.y = 0;
            transform.SetPositionXY(transform.position.x + movablePlatform.LastFrameDisplacement.x, transform.position.y + movablePlatform.LastFrameDisplacement.y);
            movablePlatformVelocity = movablePlatform.LastFrameDisplacement / Time.deltaTime;
        }
        else
        {
            float rayLength = (velocity.y < 0) ? Mathf.Max(Mathf.Abs(displacement.y), minGravityRayLenght) : minGravityRayLenght;

            boxOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y + halfColliderHeight);
            hit = Physics2D.BoxCast(boxOrigin, boxSize, 0, Vector2.down, rayLength, obstaclesMask | onewayMask);

            if (hit.collider != null && hit.collider.gameObject.layer == Layers.MovablePlatform)
            {
                movablePlatform = hit.collider.GetComponent<MoveAlongWaypoints>();
                movablePlatformVelocity = movablePlatform.LastFrameDisplacement / Time.deltaTime;
                movablePlatformCollider = hit.collider.GetComponent<Collider2D>();

                if (movablePlatformVelocity.y > velocity.y)
                {
                    Vector2 size = new Vector2(boxCollider.size.x, bias);
                    boxOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.max.y - bias);

                    hit = Physics2D.BoxCast(boxOrigin, size, 0, Vector2.down, Mathf.Infinity, movablePlatformMask);

                    OnGrounded();
                    displacement.y = 0;
                    HorizontalMovmentDirection = Vector3.Cross(hit.normal, Vector3.forward).normalized;

                    transform.SetPositionX(transform.position.x + movablePlatform.LastFrameDisplacement.x);
                    transform.SetPositionY(transform.position.y + hit.point.y - boxCollider.bounds.min.y + bias);
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

        //Vertical Movement
        if (velocity.y < Mathf.Epsilon && movablePlatform == null)
        {
            boxOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y + halfColliderHeight);
            hit = Physics2D.BoxCast(boxOrigin, boxSize, 0, Vector2.down, Mathf.Max(Mathf.Abs(displacement.y), minGravityRayLenght), obstaclesMask | onewayMask);

            if (hit.collider != null)
            {

                displacement.y = hit.point.y - boxCollider.bounds.min.y + bias;
                HorizontalMovmentDirection = Vector3.Cross(hit.normal, Vector3.forward).normalized;

                OnGrounded(Mathf.Abs(HorizontalMovmentDirection.y) < slopeLimit);
            }
        }
        else if (velocity.y > Mathf.Epsilon)
        {
            boxOrigin = new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.max.y - halfColliderHeight);

            hit = Physics2D.BoxCast(boxOrigin, boxSize, 0, Vector2.up, Mathf.Max(Mathf.Abs(displacement.y), headBias), obstaclesMask);
            if (hit.collider != null)
            {
                displacement.y = hit.point.y - boxCollider.bounds.max.y - headBias;
                velocity.y = 0;
            }
        }

        transform.SetPositionY(transform.position.y + displacement.y);

        //HorizontalMovement
        if (HorizontalMovmentDirection.x > 0)
        {
            if (Mathf.Abs(HorizontalMovmentDirection.y) < slopeLimit)
            {
                displacement = HorizontalMovmentDirection * Mathf.Sign(velocity.x) * Mathf.Abs(displacement.x);
            }
            else
            {
                HorizontalMovmentDirection *= Mathf.Sign(HorizontalMovmentDirection.y);
                displacement = HorizontalMovmentDirection * velocity.y * Time.deltaTime;
            }

            hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, displacement, displacement.magnitude, obstaclesMask);

            if (hit.collider != null)
            {
                wallDirection = -Mathf.Sign(velocity.x);
                isTouchingWall = true;
                velocity.x = 0;
                transform.SetPositionXY(transform.position.XY() + displacement.normalized * (hit.distance - bias));
            }
            else
            {
                transform.SetPositionXY(transform.position.x + displacement.x, transform.position.y + displacement.y);
            }
        }
    }

    void UnstuckSafeguard()
    {
        const float pushingForce = 20.0f;
        const float disableControlsDuration = 0.1f;

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.up, 0, obstaclesMask);

        if (hit.collider != null)
        {
            transform.SetPositionXY(preUpdatePosition);
            Push(Vector2.left * wallDirection * pushingForce, true, false, disableControlsDuration, false); 
        }
    }
}
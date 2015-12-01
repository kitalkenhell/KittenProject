using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour
{
    enum State
    {
        idle,
        walk,
        attack
    }

    const int walkingLeft = -1;
    const int walkingRight = 1;

    public Transform raycastDirection;
    public LayerMask obstacles;
    public float maxSpeed;
    public float acceleration;
    public MinMax waitTimeOnEdge;
    public float waitOnEdgeChance;
    public float randomWaitChance;
    public float randomWaitInterval;
    public MinMax randomWaitTime;
    public float attackCooldown;
    public GameObject swordsTrail;
    public GameObject longColliderSword;
    public GameObject shortColliderSword;

    Animator animator;

    int speedAnimHash;
    int attackAnimHash;
    int walkingDirection;
    State state;
    float speed;
    bool isAttacking;

    void Awake()
    {
        animator = GetComponent<Animator>();

        speedAnimHash = Animator.StringToHash("Speed");
        attackAnimHash = Animator.StringToHash("Attack");
    }

    void OnEnable()
    {
        state = State.walk;
        speed = 0;
        isAttacking = false;
        swordsTrail.SetActive(false);
        longColliderSword.SetActive(false);
        shortColliderSword.SetActive(true);
        walkingDirection = walkingRight;

        if (!Mathf.Approximately(randomWaitInterval, 0))
        {
            StartCoroutine(RandomWaiting());
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        const float wallRayDistance = 0.5f;

        if (state == State.idle || state == State.attack)
        {
            speed = Mathf.MoveTowards(speed, 0, acceleration * Time.deltaTime);
        }
        else if (state == State.walk)
        {
            RaycastHit2D hit;
            Vector2 direction = raycastDirection.position - transform.position;
            float distance = direction.magnitude;

            direction.Normalize();
            hit = Physics2D.Raycast(transform.position, direction, distance, obstacles);

            if (hit.collider == null)
            {
                if (Random.value < waitOnEdgeChance)
                {
                    StartCoroutine(Idle(waitTimeOnEdge.Random()));
                    return;
                }
                else
                {
                    QuickTurn();
                }
            }
            else if (hit.distance < distance * wallRayDistance)
            {
                QuickTurn();
            }

            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
        }

        transform.SetPositionX(transform.position.x + speed * walkingDirection * Time.deltaTime);
        animator.SetFloat(speedAnimHash, speed);
    }

    void QuickTurn()
    {
        const float deaccelerationDuration = 0.3f;

        StartCoroutine(Idle(deaccelerationDuration));
    }

    IEnumerator Idle(float time)
    {
        state = State.idle;

        yield return new WaitForSeconds(time);

        state = State.walk;
        walkingDirection *= -1;

        transform.SetScaleX(transform.localScale.x * -1);
    }

    IEnumerator RandomWaiting()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomWaitInterval);

            if (Random.value < randomWaitChance && state == State.walk)
            {
                StartCoroutine(Idle(randomWaitTime.Random()));
            }
        }
    }

    void Attack()
    {
        if (!isAttacking)
       {
            isAttacking = true;
            state = State.attack;
            animator.SetTrigger(attackAnimHash);
        }
    }

    void AttackFinished()
    {
        state = State.walk;
        swordsTrail.SetActive(false);
        longColliderSword.SetActive(false);
        shortColliderSword.SetActive(true);
        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            if (other.transform.position.x < transform.position.x)
            {
                walkingDirection = walkingLeft;
                transform.SetScaleX(-Mathf.Abs(transform.localScale.x));
            }
            else
            {
                walkingDirection = walkingRight;
                transform.SetScaleX(Mathf.Abs(transform.localScale.x));
            }

            Attack();
        }
    }

    void EnableSwordsTrail()
    {
        swordsTrail.SetActive(true);
        longColliderSword.SetActive(true);
        shortColliderSword.SetActive(false);
    }
}

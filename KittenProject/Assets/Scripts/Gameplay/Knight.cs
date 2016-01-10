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

    public Transform raycastDirection;
    public LayerMask obstacles;
    public float maxSpeed;
    public float acceleration;
    public MinMax waitTimeOnEdge;
    public float waitOnEdgeChance;
    public float attackCooldown;
    public GameObject swordsTrail;
    public GameObject longColliderSword;
    public GameObject shortColliderSword;
    public AudioSource swordSwingSound;

    Animator animator;
    SpriteTurner spriteTurner;

    int speedAnimHash;
    int attackAnimHash;
    State state;
    float speed;
    bool isAttacking;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteTurner = GetComponent<SpriteTurner>();

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
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        const float wallRayDistance = 2.0f;

        if (state == State.idle || state == State.attack)
        {
            speed = Mathf.MoveTowards(speed, 0, acceleration * Time.deltaTime);
        }
        else if (state == State.walk && !spriteTurner.Turning)
        {
            RaycastHit2D hit;
            Vector2 direction = raycastDirection.position - transform.position;
            float distance = direction.magnitude;

            direction.Normalize();
            hit = Physics2D.Raycast(transform.position, direction.normalized, distance, obstacles);

            if (hit.collider == null)
            {
                if (Random.value < waitOnEdgeChance)
                {
                    StartCoroutine(Idle(waitTimeOnEdge.Random()));
                }
                else
                {
                    QuickTurn();
                }
            }
            else if (hit.distance < wallRayDistance)
            {
                QuickTurn();
            }

            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
        }

        transform.SetPositionXY(transform.position.XY() + transform.right.XY() * speed * Time.deltaTime);
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

        spriteTurner.InstantTurn();
        state = State.walk; 
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            state = State.attack;
            animator.SetTrigger(attackAnimHash);
            StopAllCoroutines();
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
        if (other.gameObject.layer == Layers.Player && !isAttacking)
        {
            if ( Mathf.Sign(transform.position.x - other.transform.position.x) == Mathf.Sign(transform.right.x))
            {
                spriteTurner.Turn();
            }

            Attack();
        }
    }

    void EnableSwordsTrail()
    {
        swordsTrail.SetActive(true);
        longColliderSword.SetActive(true);
        shortColliderSword.SetActive(false);
        swordSwingSound.Play();
    }
}

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
    public int walkingDirection = 1;
    public float acceleration;

    Animator animator;

    int speedAnimHash;
    int attackAnimHash;
    State state;
    float speed;

    void Start()
    {
        animator = GetComponent<Animator>();

        speedAnimHash = Animator.StringToHash("Speed");
        attackAnimHash = Animator.StringToHash("Attack");
        state = State.walk;
        speed = maxSpeed;

        Attack();
    }

    void Update()
    {
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
                StartCoroutine(Idle(2.0f));
                return;
            }
            else if (hit.distance < distance * 0.7f)
            {              
                StartCoroutine(Idle(0.25f));
            }

            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);
        }

        transform.SetPositionX(transform.position.x + speed * walkingDirection * Time.deltaTime);
        animator.SetFloat(speedAnimHash, speed);
    }

    IEnumerator Idle(float time)
    {
        state = State.idle;

        yield return new WaitForSeconds(time);

        state = State.walk;
        walkingDirection *= -1;
        transform.SetScaleX(transform.localScale.x * -1);
    }

    void Attack()
    {
        state = State.attack;
        animator.SetTrigger(attackAnimHash);
    }

    void AttackFinished()
    {
        state = State.walk;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            Attack();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour
{
    public CoinEmitter coinEmitter;
    public Vector2 deathForce;
    public float destroyDelay;
    public int hp;
    public int coinDropOnHit;
    public int bonusCoinDropOnDeath;

    Animator animator;
    new Collider2D collider;
    Rigidbody2D body;
    MoveAlongWaypoints mover;

    int speedAnimHash = Animator.StringToHash("Speed");
    int deadAnimHash = Animator.StringToHash("Dead");

    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponentInChildren<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        mover = GetComponent<MoveAlongWaypoints>();
    }

    void Update()
    {
        Vector2 velocity = GetComponent<MoveAlongWaypoints>().Velocity;

        if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
        {
            transform.SetScaleX(Mathf.Abs(transform.localScale.x) * -Mathf.Sign(velocity.x));
        }

        animator.SetFloat(speedAnimHash, velocity.sqrMagnitude);
    }

    void Die()
    {
        animator.SetTrigger(deadAnimHash);
        GetComponent<MoveAlongWaypoints>().enabled = false;

        body.isKinematic = false;
        collider.enabled = false;
        body.isKinematic = false;
        body.velocity = new Vector2(deathForce.x * transform.localScale.x, deathForce.y);

        Destroy(gameObject, destroyDelay);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        ExplodeObject explodingObject = collision.gameObject.GetComponent<ExplodeObject>();

        if (explodingObject != null)
        {
            explodingObject.Explode();
            coinEmitter.transform.position = explodingObject.transform.position;
            coinEmitter.Emit(coinDropOnHit);
            --hp;

            if (hp <= 0)
            {
                coinEmitter.Emit(bonusCoinDropOnDeath);
                Die();
            }
        }
    }
}

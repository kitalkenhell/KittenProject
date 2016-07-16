using UnityEngine;
using System.Collections;

public class Dragon : MonoBehaviour
{
    const int headAnimatonLayerIndex = 1;
    const float maxWeightValue = 1.0f;
    const float minWeightValue = 0.0f;
    const float weightChangingSpeed = 2.0f;
    const float disableHeadLayerDelay = 0.5f;

    public CoinEmitter coinEmitter;
    public Vector2 deathForce;
    public float destroyDelay;
    public int hp;
    public int coinDropOnHit;
    public int bonusCoinDropOnDeath;
    public float attackCooldown;
    public float randomAttackCooldown;
    public float maxAttackAngle;
    public float maxAttackDistance;
    public LayerMask obstacles;
    public Transform fireballSource;
    public Transform fireballForce;
    public MinMax fireballForceOffset;
    public GameObject fireballPrefab;
    public float[] fireballSpawnDelays;

    Animator animator;
    new Collider2D collider;
    Rigidbody2D body;

    int speedAnimHash = Animator.StringToHash("Speed");
    int deadAnimHash = Animator.StringToHash("Dead");
    int attackAnimHash = Animator.StringToHash("Attack");

    bool canAttack;
    bool canRandomAttack;
    float headAnimatonLayerWeight;

    void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponentInChildren<Collider2D>();
        body = GetComponent<Rigidbody2D>();

        canAttack = true;
        canRandomAttack = false;
        headAnimatonLayerWeight = 0;

        StartCoroutine(RandomAttackCooldown());
    }

    void Update()
    {
        Vector2 velocity = GetComponent<MoveAlongWaypoints>().Velocity;

        if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
        {
            transform.SetScaleX(Mathf.Abs(transform.localScale.x) * -Mathf.Sign(velocity.x));
        }

        animator.SetFloat(speedAnimHash, velocity.sqrMagnitude);

        if (canAttack && transform.position.y > CoreLevelObjects.player.transform.position.y)
        {
            float angle = Vector3.Angle(fireballForce.position - fireballSource.position, CoreLevelObjects.player.transform.position - fireballSource.position);

            if (canRandomAttack || angle < maxAttackAngle && Vector3.Distance(transform.position, CoreLevelObjects.player.transform.position) < maxAttackDistance)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        StopAllCoroutines();
        StartCoroutine(AttackCooldown());
        StartCoroutine(RandomAttackCooldown());
        StartCoroutine(SpawnFireballs());
        animator.SetTrigger(attackAnimHash);
        canAttack = false;

        headAnimatonLayerWeight = minWeightValue;
        StartCoroutine(SetHeadAnimationLayerWeight(maxWeightValue));
    }

    IEnumerator SpawnFireballs()
    {
        foreach (var delay in fireballSpawnDelays)
        {
            yield return new WaitForSeconds(delay);
            SpawnFireball();
        }

        yield return new WaitForSeconds(disableHeadLayerDelay);

        headAnimatonLayerWeight = maxWeightValue;
        StartCoroutine(SetHeadAnimationLayerWeight(minWeightValue));
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator RandomAttackCooldown()
    {
        canRandomAttack = false;
        yield return new WaitForSeconds(randomAttackCooldown);
        canRandomAttack = true;
    }

    IEnumerator SetHeadAnimationLayerWeight(float targetWeight)
    {
        while (!Mathf.Approximately(headAnimatonLayerWeight, targetWeight))
        {
            headAnimatonLayerWeight = Mathf.MoveTowards(headAnimatonLayerWeight, targetWeight, Time.deltaTime * weightChangingSpeed);
            animator.SetLayerWeight(headAnimatonLayerIndex, headAnimatonLayerWeight);
            
            yield return null;
        }
    }

    void SpawnFireball()
    {
        Rigidbody2D fireball = (Instantiate(fireballPrefab, fireballSource.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
        fireball.velocity = fireballForce.position - fireballSource.position + Vector3.up * fireballForceOffset.Random();
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

    void OnCollisionEnter2D(Collision2D collision)
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

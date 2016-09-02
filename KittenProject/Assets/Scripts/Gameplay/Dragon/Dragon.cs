using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Dragon : MonoBehaviour
{
    const int headAnimatonLayerIndex = 1;
    const float maxWeightValue = 1.0f;
    const float minWeightValue = 0.0f;
    const float weightChangingSpeed = 2.0f;
    const float disableHeadLayerDelay = 0.5f;

    public CoinEmitter coinEmitter;
    public AudioSource breathFireSound;
    public Vector2 deathForce;
    public float destroyDelay;
    public int hp;
    public int coinDropOnHit;
    public int bonusCoinDropOnDeath;
    public bool facePlayerWhenNotMoving;
    public float attackCooldown;
    public float randomAttackCooldown;
    public float maxAttackAngle;
    public float maxAttackDistance;
    public LayerMask obstacles;
    public Transform fireballSource;
    public Transform randomAttackfireballForce;
    public MinMax fireballForceOffset;
    public GameObject fireballPrefab;
    public float[] fireballSpawnDelays;
    public float attackPlayerForceMagnitude;
    public float breathFireSoundDelay;
    public UnityEvent onKilled;

    Animator animator;
    Collider2D dragonCollider;
    Rigidbody2D body;
    MoveAlongWaypoints mover;

    int speedAnimHash = Animator.StringToHash("Speed");
    int deadAnimHash = Animator.StringToHash("Dead");
    int attackAnimHash = Animator.StringToHash("Attack");
    int hitAnimHash = Animator.StringToHash("Hit");

    bool attackIsOnCooldown;
    bool randomAttackIsOnCooldown;
    float headAnimatonLayerWeight;

    void Awake()
    {
        animator = GetComponent<Animator>();
        dragonCollider = GetComponentInChildren<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        mover = GetComponent<MoveAlongWaypoints>();

        attackIsOnCooldown = true;
        randomAttackIsOnCooldown = false;
        headAnimatonLayerWeight = 0;

        StartCoroutine(RandomAttackCooldown());
    }

    void Update()
    {
        Vector2 velocity = mover.Velocity;

        if (Mathf.Abs(velocity.x) > Mathf.Epsilon)
        {
            transform.SetScaleX(Mathf.Abs(transform.localScale.x) * -Mathf.Sign(velocity.x));
        }
        else if (facePlayerWhenNotMoving)
        {
            transform.SetScaleX(Mathf.Abs(transform.localScale.x) * Mathf.Sign(transform.position.x - CoreLevelObjects.player.transform.position.x));
        }

        animator.SetFloat(speedAnimHash, velocity.sqrMagnitude);

        if (attackIsOnCooldown && transform.position.y > CoreLevelObjects.player.transform.position.y)
        {
            float angle = Vector3.Angle(randomAttackfireballForce.position - fireballSource.position, CoreLevelObjects.player.transform.position - fireballSource.position);

            if (angle < maxAttackAngle && Vector3.Distance(transform.position, CoreLevelObjects.player.transform.position) < maxAttackDistance)
            {
                Attack(true);
            }
            else if (randomAttackIsOnCooldown)
            {
                Attack(false);
            }
        }
    }

    void Attack(bool aimPlayer)
    {
        StopAllCoroutines();
        StartCoroutine(AttackCooldown());
        StartCoroutine(RandomAttackCooldown());
        StartCoroutine(SpawnFireballs(aimPlayer));
        animator.SetTrigger(attackAnimHash);
        attackIsOnCooldown = false;

        headAnimatonLayerWeight = minWeightValue;
        StartCoroutine(SetHeadAnimationLayerWeight(maxWeightValue));
    }

    IEnumerator SpawnFireballs(bool aimPlayer)
    {
        breathFireSound.PlayDelayed(breathFireSoundDelay);

        foreach (var delay in fireballSpawnDelays)
        {
            yield return new WaitForSeconds(delay);
            SpawnFireball(aimPlayer);
        }

        yield return new WaitForSeconds(disableHeadLayerDelay);

        headAnimatonLayerWeight = maxWeightValue;
        StartCoroutine(SetHeadAnimationLayerWeight(minWeightValue));
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackIsOnCooldown = true;
    }

    IEnumerator RandomAttackCooldown()
    {
        randomAttackIsOnCooldown = false;
        yield return new WaitForSeconds(randomAttackCooldown);
        randomAttackIsOnCooldown = true;
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

    void SpawnFireball(bool aimPlayer)
    {
        Rigidbody2D fireball = (Instantiate(fireballPrefab, fireballSource.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();

        if (aimPlayer)
        {
            fireball.velocity = (CoreLevelObjects.player.transform.position - fireballSource.position).normalized * attackPlayerForceMagnitude + Vector3.up * fireballForceOffset.Random();
        }
        else
        {
            fireball.velocity = randomAttackfireballForce.position - fireballSource.position + Vector3.up * fireballForceOffset.Random();
        }
    }

    void Die()
    {
        animator.SetTrigger(deadAnimHash);
        mover.enabled = false;

        body.isKinematic = false;
        dragonCollider.enabled = false;
        body.isKinematic = false;
        body.velocity = new Vector2(deathForce.x * transform.localScale.x, deathForce.y);

        if (onKilled != null)
        {
            onKilled.Invoke();
        }

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
            else
            {
                animator.SetTrigger(hitAnimHash);
            }
        }
    }
}

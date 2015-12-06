using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{
    public int coinsDropRate;
    public float invulnerableDuration;
    public int maxHealth;
    public GameObject[] objectsToDisableOnDeath;
    public GameObject[] objectsToEnableOnDeath;
    public Animator animator;
    public float angularSpeedOnDeath;
    public Vector2 velocityOnDeath;
    public Vector2 velocityOnHellfireDeath;

    CoinEmitter coinEmitter;
    PlayerController controller;
    Rigidbody2D body;
    Collider2D collder;
    
    int coins;
    bool isInvulnerable;
    int health;

    public bool IsAlive
    {
        get
        {
            return health > 0;
        }
    }

    void Start()
    {
        coinEmitter = GetComponent<CoinEmitter>();
        controller = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody2D>();
        collder = GetComponent<Collider2D>();

        isInvulnerable = false;
        coins = 0;
        health = maxHealth;

        PostOffice.coinCollected += OnCoinCollected;
    }

    void OnDestroy()
    {
        PostOffice.coinCollected -= OnCoinCollected;
    }

    IEnumerator InvulnerableCountdown()
    {
        yield return new WaitForSeconds(invulnerableDuration);
        isInvulnerable = false;
    }

    void OnCoinCollected(int amount)
    {
        coins += amount;
    }

    public void DealDamage(int damage)
    {
        if (!isInvulnerable && IsAlive)
        {
            int drop = Mathf.Min(coins, coinsDropRate);

            health -= damage;
            coins -= drop;
            coinEmitter.Emit(drop);
            isInvulnerable = true;
            StartCoroutine(InvulnerableCountdown());

            if (health <= 0)
            {
                OnDeath(false);
            }
        }
    }

    public void DeathByHellfire()
    {
        if (IsAlive)
        {
            health = 0;
            OnDeath(true); 
        }
    }

    void OnDeath(bool useHellfireDeathAnimation)
    {
        foreach (var objectToDisable in objectsToDisableOnDeath)
        {
            objectToDisable.SetActive(false);
        }

        foreach (var objectToEnable in objectsToEnableOnDeath)
        {
            objectToEnable.SetActive(true);
        }

        controller.enabled = false;
        body.isKinematic = false;
        body.gravityScale = 1.0f;
        gameObject.layer = Layers.Default;
        body.angularVelocity = angularSpeedOnDeath;
        animator.enabled = false;

        if (useHellfireDeathAnimation)
        {
            body.velocity = new Vector2(Mathf.Sign(controller.Velocity.x) * velocityOnHellfireDeath.x, velocityOnHellfireDeath.y);
        }
        else
        {
            body.velocity = new Vector2(Mathf.Sign(controller.Velocity.x) * Mathf.Max(Mathf.Abs(controller.Velocity.x), 
                Mathf.Abs(velocityOnDeath.x)), Mathf.Max(controller.Velocity.y, velocityOnDeath.y));
            collder.isTrigger = false;
        }

        PostOffice.PostPlayerDied();
    }
}

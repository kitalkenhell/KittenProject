using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour
{
    public int coinsDropRate;
    public float invulnerableDuration;
    public GameObject[] objectsToDisableOnDeath;
    public GameObject[] objectsToEnableOnDeath;
    public Transform parachute;
    public Animator animator;
    public float angularSpeedOnDeath;
    public Vector2 velocityOnDeath;
    public Vector2 velocityOnHellfireDeath;
    public float parachuteClosingSpeedOnHellfireDeath;

    CoinEmitter coinEmitter;
    PlayerController controller;
    Rigidbody2D body;
    Collider2D collder;
    
    bool isInvulnerable;

    int victoryAnimHash;

    public int Coins
    {
        get;
        private set;
    }

    public bool GoldenKittenCollected
    {
        get;
        private set;
    }

    public int Health
    {
        get;
        private set;
    }

    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
        
    }

    void Awake()
    {
        CoreLevelObjects.player = this;
    }

    void Start()
    {
        coinEmitter = GetComponent<CoinEmitter>();
        controller = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody2D>();
        collder = GetComponent<Collider2D>();

        victoryAnimHash = Animator.StringToHash("Victory");
        isInvulnerable = false;
        Coins = 0;
        Health = GameSettings.MaxPlayerHealth;

        PostOffice.coinCollected += OnCoinCollected;
        PostOffice.heartCollected += OnHeartCollected;
        PostOffice.goldenKittenCollected += OnGoldenKittenCollected;
        PostOffice.victory += OnVictory;
    }

    void OnDestroy()
    {
        PostOffice.coinCollected -= OnCoinCollected;
        PostOffice.heartCollected -= OnHeartCollected;
        PostOffice.goldenKittenCollected -= OnGoldenKittenCollected;
        PostOffice.victory -= OnVictory;
    }

    IEnumerator InvulnerableCountdown()
    {
        yield return new WaitForSeconds(invulnerableDuration);
        isInvulnerable = false;
    }

    IEnumerator ScaleDownParachute()
    {
        while (parachute.localScale.x > Mathf.Epsilon)
        {
            parachute.localScale = Vector3.MoveTowards(parachute.localScale, Vector3.zero, Time.deltaTime * parachuteClosingSpeedOnHellfireDeath);
            yield return null;
        }
    }

    void OnCoinCollected(int amount)
    {
        Coins += amount;
    }

    void OnHeartCollected()
    {
        if (Health < GameSettings.MaxPlayerHealth)
        {
            PostOffice.PostPlayerHealthChanged(Health, ++Health);
        }
    }

    void OnGoldenKittenCollected()
    {
        GoldenKittenCollected = true;
    }

    void DropCoins()
    {
        int drop = Mathf.Min(Coins, coinsDropRate);

        Coins -= drop;
        coinEmitter.Emit(drop);

        PostOffice.PostCoinDropped(drop);
    }

    public void DealDamage(int damage)
    {
        if (!isInvulnerable && IsAlive)
        {
            Health -= damage;
            
            isInvulnerable = true;

            DropCoins();
            StartCoroutine(InvulnerableCountdown());

            PostOffice.PostPlayerHealthChanged(Health + damage, Health);

            if (Health <= 0)
            {
                OnDeath(false);
            }
        }
    }

    public void DeathByHellfire()
    {
        if (IsAlive)
        {
            DropCoins();
            Health = 0;
            OnDeath(true); 
        }
    }

    void OnVictory()
    {
        controller.enabled = false;
        animator.SetTrigger(victoryAnimHash);
        StartCoroutine(ScaleDownParachute());
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
        StartCoroutine(ScaleDownParachute());

        AnalyticsManager.OnLevelFailed(SceneManager.GetActiveScene().name, LevelStopwatch.Stopwatch, transform.position, useHellfireDeathAnimation);

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

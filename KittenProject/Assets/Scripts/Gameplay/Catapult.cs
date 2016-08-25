using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    const float forceMultiplier = 5.0f;

    public float interval = 5.0f;
    public float delay;
    public MinMax randomForceMultiplier;
    public MinMax rockAngularVelocity;
    public MinMax rockInitialSpeedRange = new MinMax(0, Mathf.Infinity);
    public Transform rockPlacement;
    public Transform rockForce;
    public GameObject RockPrefab;
    public AudioSource instantiateRockSound;
    public AudioSource fireSound;
    public bool canShoot = true;

    Animator animator;

    int fireAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        fireAnimHash = Animator.StringToHash("Fire");
    }

    void OnEnable()
    {
        StartCoroutine(Fire());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!canShoot)
            {
                yield return new WaitUntil(() => { return canShoot;  });
            }

            animator.SetTrigger(fireAnimHash);

            if (fireSound != null)
            {
                fireSound.Play();
            }

            yield return new WaitForSeconds(interval);
        }
    }

    void InstantiateRock()
    {
        Rigidbody2D rock;
        float magnitude;

        rock = (Instantiate(RockPrefab, rockPlacement.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
        rock.velocity = (rockForce.position - rockPlacement.position) * forceMultiplier * randomForceMultiplier.Random();

        magnitude = rock.velocity.magnitude;

        if (magnitude > rockInitialSpeedRange.max)
        {
            rock.velocity = rock.velocity.normalized * rockInitialSpeedRange.max;
        }
        else if (magnitude < rockInitialSpeedRange.min)
        {
            rock.velocity = rock.velocity.normalized * rockInitialSpeedRange.min;
        }

        rock.angularVelocity = rockAngularVelocity.Random() * Utils.RandomSign();

        if (instantiateRockSound != null)
        {
            instantiateRockSound.Play(); 
        }
    }

}

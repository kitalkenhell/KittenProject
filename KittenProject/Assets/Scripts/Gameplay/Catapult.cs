using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    const float forceMultiplier = 5.0f;

    public float interval = 5.0f;
    public float delay;
    public MinMax randomForceMultiplier;
    public MinMax rockAngularVelocity;
    public Transform rockPlacement;
    public Transform rockForce;
    public GameObject RockPrefab;
    public AudioSource instantiateRockSound;
    public AudioSource fireSound;

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

        rock = (Instantiate(RockPrefab, rockPlacement.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
        rock.velocity = (rockForce.position - rockPlacement.position) * forceMultiplier * randomForceMultiplier.Random();

        rock.angularVelocity = rockAngularVelocity.Random() * Utils.RandomSign();

        if (instantiateRockSound != null)
        {
            instantiateRockSound.Play(); 
        }
    }

}

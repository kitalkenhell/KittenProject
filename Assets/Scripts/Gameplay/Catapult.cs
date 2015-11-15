using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    const float forceMultiplier = 5.0f;

    public float interval = 5.0f;
    public float instantiateRockDelay = 0.2f;
    public MinMax randomForceMultiplier;
    public MinMax rockAngularVelocity;
    public Transform rockPlacement;
    public Transform rockForce;
    public GameObject RockPrefab;

    Animator animator;

    int fireAnimHash;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        fireAnimHash = Animator.StringToHash("Fire");

        StartCoroutine(Fire());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Fire()
    {
        while (true)
        {
            animator.SetTrigger(fireAnimHash);
            StartCoroutine(InstantiateRock());
            yield return new WaitForSeconds(interval); 
        }
    }

    IEnumerator InstantiateRock()
    {
        Rigidbody2D rock;

        yield return new WaitForSeconds(instantiateRockDelay);

        rock = (Instantiate(RockPrefab, rockPlacement.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
        rock.velocity = (rockForce.position - rockPlacement.position) * 
            forceMultiplier * Random.Range(randomForceMultiplier.min, randomForceMultiplier.max);

        rock.angularVelocity = Random.Range(rockAngularVelocity.min, rockAngularVelocity.max) * Utils.RandomSign();
    }

}

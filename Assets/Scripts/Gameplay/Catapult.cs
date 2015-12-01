using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    const float forceMultiplier = 5.0f;

    public float interval = 5.0f;
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
            yield return new WaitForSeconds(interval); 
        }
    }

    void InstantiateRock()
    {
        Rigidbody2D rock;

        rock = (Instantiate(RockPrefab, rockPlacement.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
        rock.velocity = (rockForce.position - rockPlacement.position) * forceMultiplier * randomForceMultiplier.Random();

        rock.angularVelocity = rockAngularVelocity.Random() * Utils.RandomSign();
    }

}

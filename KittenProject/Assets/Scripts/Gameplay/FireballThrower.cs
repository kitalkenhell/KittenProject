using UnityEngine;
using System.Collections;

public class FireballThrower : MonoBehaviour
{
    const float forceMultiplier = 3.0f;

    public float startDelay;
    public MinMax spawnInterval;
    public MinMax randomForceMultiplier;
    public GameObject fireballPrefab;
    public Transform fireballForce;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        Rigidbody2D fireball;

        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            fireball = (Instantiate(fireballPrefab, transform.position, Quaternion.identity) as GameObject).GetComponentInChildren<Rigidbody2D>();
            fireball.velocity = (fireballForce.position - transform.position) * forceMultiplier * randomForceMultiplier.Random();
            yield return new WaitForSeconds(spawnInterval.Random());
        }
    }
}

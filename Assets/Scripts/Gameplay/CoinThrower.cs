using UnityEngine;
using System.Collections;

public class CoinThrower : MonoBehaviour 
{
    public MinMax interval;
    public MinMax angularSpeed;

    public Transform forceBoundsA;
    public Transform forceBoundsB;

    public CoinFactory coinFactory;

    Vector2 forceA;
    Vector2 forceB;

	void OnEnable() 
    {
        StartCoroutine(Emit());
	}

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator Emit()
    {
        while (true)
        {
            const float amplification = 3.0f;

            forceA = ((forceBoundsA.position - transform.position).XY()) * amplification;
            forceB = ((forceBoundsB.position - transform.position).XY()) * amplification;

            float factor = Random.value;
            Vector2 velocity = forceA * factor + forceB * (1 - factor);

            coinFactory.Spawn(transform.position, velocity, Random.Range(angularSpeed.min, angularSpeed.max) * Utils.RandomSign());

            yield return new WaitForSeconds(Random.Range(interval.min, interval.max));
        }
    }
	

}

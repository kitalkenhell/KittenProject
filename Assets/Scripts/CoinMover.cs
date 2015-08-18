using UnityEngine;
using System.Collections;

public class CoinMover : MonoBehaviour 
{
    public float speed;
    public float shrinkingSpeed;
    public Transform target;

	void Update ()
    {
        const float scaleThreshold = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.localScale -= Vector3.one * shrinkingSpeed * Time.deltaTime;

        if (transform.localScale.x < scaleThreshold)
        {
            Destroy(gameObject);
        }
	
	}
}

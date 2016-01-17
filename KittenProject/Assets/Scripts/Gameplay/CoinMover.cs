using UnityEngine;
using System.Collections;

public class CoinMover : MonoBehaviour 
{
    public float speed;
    public float shrinkingSpeed;
    public Transform sprite;
    public Animator pickUpEffect;
    public Transform target;
    public bool destroy;

    void OnEnable()
    {
        pickUpEffect.transform.parent = null;
        pickUpEffect.enabled = true;
    }

	void Update()
    {
        const float scaleThreshold = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        sprite.localScale -= Vector3.one * shrinkingSpeed * Time.deltaTime;

        if (sprite.localScale.x < scaleThreshold)
        {
            PostOffice.PostCoinCollected();

            if (destroy)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
	
	}
}

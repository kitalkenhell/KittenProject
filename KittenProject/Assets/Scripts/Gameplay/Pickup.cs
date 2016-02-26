using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    public enum Type
    {
        coin,
        heart
    }

    public float speed;
    public float shrinkingSpeed;
    public Transform sprite;
    public Animator pickUpEffect;
    public bool destroy;
    public Type type;

    void OnEnable()
    {
        pickUpEffect.transform.parent = null;
        pickUpEffect.enabled = true;
    }

	void Update()
    {
        const float scaleThreshold = 0.5f;

        transform.position = Vector3.MoveTowards(transform.position, CoreLevelObjects.player.transform.position, speed * Time.deltaTime);
        sprite.localScale -= Vector3.one * shrinkingSpeed * Time.deltaTime;

        if (sprite.localScale.x < scaleThreshold)
        {
            if (type == Type.coin)
            {
                PostOffice.PostCoinCollected();
            }
            else
            {
                PostOffice.PostHeartCollected();
            }

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

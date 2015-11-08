using UnityEngine;
using System.Collections;

public class BurningPlatform : MonoBehaviour 
{
    public float delay;
    public float decreaseRate;
    public float decreaseDelay;
    public float disableControlsDuration;
    public int damage;
    public float maxScale;
    public AnimationCurve scaleCurve;
    public Transform force;

    float counter;
    float decreaseCounter;
    bool active;

    void Start()
    {
        counter = 0;
        decreaseCounter = 0;
        active = true;
    }

    IEnumerator DecreaseCounter()
    {
        while (counter > 0)
        {
            decreaseCounter -= Time.fixedDeltaTime;
   
            if (decreaseCounter < 0)
            {
                counter -= Time.fixedDeltaTime * decreaseRate;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        active = true;
        StopAllCoroutines();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        counter += Time.fixedDeltaTime;

        if (counter > delay && active)
        {
            const float amplification = 3.0f;

            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                active = false;
                player.Hit(damage);
                player.Push((force.position - transform.position).XY() * amplification, true, true, disableControlsDuration);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        decreaseCounter = decreaseDelay;
        StartCoroutine(DecreaseCounter());
    }
}

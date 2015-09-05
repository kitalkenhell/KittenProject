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
    public Transform leftLeg;
    public Transform rightLeg;
    public GameObject leftFlame;
    public GameObject rightFlame;

    float counter;
    float decreaseCounter;
    bool active;

    void Start()
    {
        counter = 0;
        decreaseCounter = 0;
        active = true;
        leftFlame.SetActive(false);
        rightFlame.SetActive(false);
    }

    void Update()
    {
        UpdateFlames();
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

        leftFlame.SetActive(false);
        rightFlame.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        active = true;
        StopAllCoroutines();
        leftFlame.SetActive(true);
        rightFlame.SetActive(true);
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
                player.Push((force.position - transform.position).xy() * amplification, true, true, disableControlsDuration);
            }
        }
    }

    void UpdateFlames()
    {
        float targetScale = counter / delay;
        targetScale = scaleCurve.Evaluate(targetScale);

        leftFlame.transform.position = leftLeg.position;
        rightFlame.transform.position = rightLeg.position;

        leftFlame.transform.localScale = Vector3.one * targetScale * maxScale;
        rightFlame.transform.localScale = Vector3.one * targetScale * maxScale;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        decreaseCounter = decreaseDelay;
        StartCoroutine(DecreaseCounter());
    }
}

using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{
    public float interval;
    public float offset;
    public float soundDelay;
    public AudioSource audioSource;

    Animation animaton;

	void Awake()
    {
        animaton = GetComponent<Animation>();
    }

    void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(offset);

        while (true)
        {
            animaton.Play();
            audioSource.PlayDelayed(soundDelay);
            yield return new WaitForSeconds(interval);
        }

    }
}

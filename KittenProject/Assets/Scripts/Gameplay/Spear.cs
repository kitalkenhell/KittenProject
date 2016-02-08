using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour
{
    public float interval;
    public float offset;

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
            yield return new WaitForSeconds(interval);
            animaton.Play();
        }

    }
}

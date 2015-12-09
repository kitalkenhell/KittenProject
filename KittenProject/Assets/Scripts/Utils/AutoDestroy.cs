using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float delay;

	void Start ()
    {
        StartCoroutine(DestroyObject());
	}

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}

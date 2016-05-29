using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
{
    public float delay;

	void Start ()
    {
        Destroy(gameObject, delay);
	}
}

using UnityEngine;
using System.Collections;

public class ExplodeObject : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject[] objectsToDestroy;

    public void Explode()
    {
        particles.Play();
        particles.transform.parent = null;

        if (objectsToDestroy != null)
        {
            foreach (var gameObject in objectsToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}

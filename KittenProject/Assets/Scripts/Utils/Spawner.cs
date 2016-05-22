using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject original;
    public float interval;
    public float delay;

    string objectName;

    void Start()
    {
        objectName = original.name;

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (original != null)
            {
                original = Instantiate(original, transform.position, transform.rotation) as GameObject;
                original.name = objectName; 
            }
            yield return new WaitForSeconds(interval);
        }
    }
}

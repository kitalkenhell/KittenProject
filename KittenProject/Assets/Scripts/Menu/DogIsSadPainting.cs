using UnityEngine;
using System.Collections;

public class DogIsSadPainting : MonoBehaviour
{
    public MinMax thunderStrikeInterval;
    public Animator[] thunders;

    int thunderStrikeAnimHash;

    void Start()
    {
        thunderStrikeAnimHash = Animator.StringToHash("Strike");

        StartCoroutine(ThunderStrikeDelay());
    }

    IEnumerator ThunderStrikeDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(thunderStrikeInterval.Random());
            thunders[Random.Range(0, thunders.Length)].SetTrigger(thunderStrikeAnimHash);

        }
    }
}

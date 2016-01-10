using UnityEngine;
using System.Collections;

public class Guillotine : MonoBehaviour
{
    public float interval;
    public float force;
    public Animator animator;

    int cutAnimHash;

    void Start()
    {
        cutAnimHash = Animator.StringToHash("Cut");
        StartCoroutine(Cut());
    }

    IEnumerator Cut()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            animator.SetTrigger(cutAnimHash);
        }

    }
}

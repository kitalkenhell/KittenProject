using UnityEngine;
using System.Collections;

public class Guillotine : MonoBehaviour
{
    public float interval;
    public float force;
    public Animator animator;
    public AudioSource audioSource;
    public float soundDelay;

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
            audioSource.PlayDelayed(soundDelay);
        }

    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}

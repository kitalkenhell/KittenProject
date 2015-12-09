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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            player.PushAndHit(Vector3.down * force);
        }
    }
}

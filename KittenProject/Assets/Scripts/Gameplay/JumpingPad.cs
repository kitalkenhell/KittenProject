using UnityEngine;
using System.Collections;

public class JumpingPad : MonoBehaviour 
{
    public Transform force;
    public float disableControlsDuration;

    Animator animator;

    int jumpAnimHash;
    bool active;

    void Start()
    {
        animator = GetComponent<Animator>();

        jumpAnimHash = Animator.StringToHash("Jump");
        active = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        const float amplification = 3.0f;

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && active)
        {
            active = false;
            animator.SetTrigger(jumpAnimHash);
            player.Push((force.position - transform.position).XY() * amplification, true, true, disableControlsDuration);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        active = true;
    }

}

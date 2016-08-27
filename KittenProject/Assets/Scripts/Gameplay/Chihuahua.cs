using UnityEngine;
using System.Collections;

public class Chihuahua : MonoBehaviour
{
    const float runnningSpeed = 2.0f;

    public Animator animator;

    int SpeedAnimHash = Animator.StringToHash("Speed");
    int jumpAnimHash = Animator.StringToHash("Jump");

    public void Run()
    {
        animator.SetFloat(SpeedAnimHash, runnningSpeed);
    }

    public void Idle()
    {
        animator.SetFloat(SpeedAnimHash, 0);
    }

    public void Jump()
    {
        animator.SetTrigger(jumpAnimHash);
    }
}

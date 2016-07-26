using UnityEngine;
using System.Collections;

public class DogCutsceneController : MonoBehaviour
{
    public Animator animator;
    public Transform parachute;

    int speedAnimHash;
    int fallingSpeedAnimHash;
    int isGroundedAnimHash;

    void Start()
    {
        speedAnimHash = Animator.StringToHash("Speed");
        fallingSpeedAnimHash = Animator.StringToHash("FallingSpeed");
        isGroundedAnimHash = Animator.StringToHash("IsGrounded");

        animator.SetFloat(speedAnimHash, 0);
        animator.SetFloat(fallingSpeedAnimHash, 0);
        animator.SetBool(isGroundedAnimHash, true);

        parachute.localScale = Vector3.zero;
    }
}
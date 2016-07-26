using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour 
{
    Animator animator;

    int nextShotAnimHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        nextShotAnimHash = Animator.StringToHash("NextShot");
    }

    public void NextShot()
    {
        animator.SetTrigger(nextShotAnimHash);
    }
}

using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour 
{
    Animator animator;

    int nextShotAnimHash;
    int skipAnimHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        nextShotAnimHash = Animator.StringToHash("NextShot");
        skipAnimHash = Animator.StringToHash("Skip");
    }

    public void NextShot()
    {
        animator.SetTrigger(nextShotAnimHash);
    }

    public void Skip()
    {
        animator.SetTrigger(skipAnimHash);
    }

    public void Finish()
    {
        animator.enabled = false;
    }
}

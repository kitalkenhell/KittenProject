using UnityEngine;
using System.Collections;

public class CutsceneCamera : MonoBehaviour 
{
    Animator animator;

    int nextShotAnimHash;
    int skipAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        nextShotAnimHash = Animator.StringToHash("NextShot");
        skipAnimHash = Animator.StringToHash("Skip");
    }

    void OnEnable()
    {
        animator.enabled = true;
    }

    void OnDisable()
    {
        animator.enabled = false;
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

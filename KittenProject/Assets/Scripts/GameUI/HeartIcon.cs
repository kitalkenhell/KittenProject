using UnityEngine;
using System.Collections;

public class HeartIcon : MonoBehaviour 
{
    public GameObject leftChunk;
    public GameObject rightChunk;
    public GameObject heart;

    Animator animator;

    int showAnimHash;
    int hideAnimHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        showAnimHash = Animator.StringToHash("Show");
        hideAnimHash = Animator.StringToHash("Hide");
    }

    public void Hide()
    {
        animator.SetTrigger(hideAnimHash);
    }

    public void Show()
    {
        animator.SetTrigger(showAnimHash);
    }
}

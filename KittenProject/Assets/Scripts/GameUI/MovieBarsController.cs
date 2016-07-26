using UnityEngine;
using System.Collections;

public class MovieBarsController : MonoBehaviour
{
    Animator animator;

    int showAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        showAnimHash = Animator.StringToHash("Show");
    }

    public void Show()
    {
        animator.SetBool(showAnimHash, true);
    }

    public void Hide()
    {
        animator.SetBool(showAnimHash, false);
    }
}

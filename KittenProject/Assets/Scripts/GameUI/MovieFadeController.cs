using UnityEngine;
using System.Collections;

public class MovieFadeController : MonoBehaviour
{
    Animator animator;

    int showAnimHash;

    public bool IsFaded
    {
        get;
        private set;
    }

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

    public void FadedEvent()
    {
        IsFaded = true;
    }

    public void UnfadedEvent()
    {
        IsFaded = false;
    }
}

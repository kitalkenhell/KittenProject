using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour
{
    Animator animator;

    int showHudAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        showHudAnimHash = Animator.StringToHash("ShowHud");
    }

    public void Show()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetBool(showHudAnimHash, true); 
        }
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetBool(showHudAnimHash, false); 
        }
    }
}

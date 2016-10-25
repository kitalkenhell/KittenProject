using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour
{
    public InputManager inputManager;

    Animator animator;

    int showHudAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        showHudAnimHash = Animator.StringToHash("ShowHud");
    }

    public void OnEnable()
    {
        inputManager.Reset();
    }

    public void Show()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetBool(showHudAnimHash, true);
            inputManager.Reset();
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

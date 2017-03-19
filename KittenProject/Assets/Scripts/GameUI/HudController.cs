using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour
{
    public InputManager inputManager;

    Animator animator;

    int showHudAnimHash;

    public bool IsVisible
    {
        get;
        private set;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();

        showHudAnimHash = Animator.StringToHash("ShowHud");
    }

    void OnEnable()
    {
        IsVisible = true;
        inputManager.Reset();
    }

    void OnDisable()
    {
        IsVisible = false;
    }

    public void Show()
    {
        if (gameObject.activeInHierarchy)
        {
            animator.SetBool(showHudAnimHash, true);
            IsVisible = true;
            inputManager.Reset();
        }
    }

    public void Hide()
    {
        if (gameObject.activeInHierarchy)
        {
            IsVisible = false;
            animator.SetBool(showHudAnimHash, false); 
        }
    }
}

using UnityEngine;
using System.Collections;

public class FreeGemsButton : MonoBehaviour
{
    Animator animator;

    int visibleAnimHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        visibleAnimHash = Animator.StringToHash("Visible");
    }

    void Update()
    {
        animator.SetBool(visibleAnimHash,(AdManager.Instance.IsVideoAdAvailable));
    }

    public void OnClick()
    {
        AdManager.Instance.ShowVideoAd();
    }
}

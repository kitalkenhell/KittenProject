using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToContinuePanel : MonoBehaviour
{
    Animator animator;

    int showAnimHash = Animator.StringToHash("Show");

    bool isVisible = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        if (!isVisible)
        {
            isVisible = true;
            animator.SetBool(showAnimHash, true);
            StartCoroutine(WaitForTap());
        }
    }

    public void Hide()
    {
        animator.SetBool(showAnimHash, false);
        StopAllCoroutines();
        isVisible = false;
    }

    IEnumerator WaitForTap()
    {
        const float delay = 0.3f;
        const int leftMouseButton = 0;

        yield return new WaitForSeconds(delay);
        yield return new WaitUntil(() => Input.touchCount > 0 || Input.GetMouseButton(leftMouseButton));

        animator.SetBool(showAnimHash, false);
        isVisible = false;
    }
}

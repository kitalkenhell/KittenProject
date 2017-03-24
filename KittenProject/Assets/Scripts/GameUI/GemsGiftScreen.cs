using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemsGiftScreen : MonoBehaviour
{
    public Text rewardLabel;

    int stepAnimHash;

    Animator animator;

    public int Reward
    {
        get;
        set;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();

        stepAnimHash = Animator.StringToHash("Step");
    }

    public void OnOpenGiftClicked()
    {
        animator.SetTrigger(stepAnimHash);
        rewardLabel.text = string.Format(Strings.GiftScreen.gemsReward, Reward);
    }

    public void OnCloseButtonClicked()
    {
        animator.SetTrigger(stepAnimHash);
    }

    public void OnClosedAnimationFinished()
    {
        gameObject.SetActive(false);
    }
}

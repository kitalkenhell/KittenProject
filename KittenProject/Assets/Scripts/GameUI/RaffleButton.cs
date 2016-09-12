using UnityEngine;
using System.Collections;

public class RaffleButton : MonoBehaviour
{
    public const int coinsToEnter = 250;

    public PlayerItems items;
    public GameObject raffleScreen;
    public GameObject freePanel;
    public GameObject paidPanel;
    public LevelProperties raffleUnlockLevel;

    public bool AlreadyOpenedGift
    {
        get;
        private set;
    }   

    int hideAnimHash;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        hideAnimHash = Animator.StringToHash("Hide");

        paidPanel.SetActive(PersistentData.HasOpenedFreeGift);
        freePanel.SetActive(!PersistentData.HasOpenedFreeGift);
        AlreadyOpenedGift = false;
    }

    public void OnEnterRaffleButtonClicked()
    {
        if (PersistentData.HasOpenedFreeGift)
        {
            PersistentData.Coins -= coinsToEnter;
            SocialManager.UnlockAchievement(SocialManager.Achievements.giftBought);
        }
        else
        {
            PersistentData.HasOpenedFreeGift = true;
        }

        AlreadyOpenedGift = true;
        animator.SetTrigger(hideAnimHash);
        raffleScreen.SetActive(true);
    }

    public bool CanEnter()
    {
        return !items.HasAllSkins() && ((raffleUnlockLevel.IsCompleted && !PersistentData.HasOpenedFreeGift) || PersistentData.Coins >= coinsToEnter);
    }
}

using UnityEngine;
using System.Collections;

public class RaffleButton : MonoBehaviour
{
    public const int coinsToEnter = 250;

    public PlayerItems items;
    public GameObject raffleScreen;

    int hideAnimHash;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        hideAnimHash = Animator.StringToHash("Hide");
    }

    public void OnEnterRaffleButtonClicked()
    {
        PersistentData.Coins -= coinsToEnter;
        animator.SetTrigger(hideAnimHash);
        raffleScreen.SetActive(true);
    }

    public bool CanEnter()
    {
        return PersistentData.Coins >= coinsToEnter && !items.HasAllSkins();
    }
}

using UnityEngine;
using System.Collections;

public class FreeGemsButton : MonoBehaviour
{
    public GemsGiftScreen gemsGiftScreen;

    Animator animator;

    int visibleAnimHash;

    void Start()
    {
        animator = GetComponent<Animator>();

        visibleAnimHash = Animator.StringToHash("Visible");
    }

    void OnDestroy()
    {
        PostOffice.videoAdWatched -= OnVideoAdWatched;
    }

    void Update()
    {
        animator.SetBool(visibleAnimHash,(AdManager.Instance.IsVideoAdAvailable));
    }

    public void OnClick()
    {
        PostOffice.videoAdWatched -= OnVideoAdWatched;
        PostOffice.videoAdWatched += OnVideoAdWatched;

        AdManager.Instance.ShowVideoAd();
    }

    public void OnVideoAdWatched()
    {
        gemsGiftScreen.Reward = AdManager.defaultVideoAdReward;
        gemsGiftScreen.gameObject.SetActive(true);

        PostOffice.videoAdWatched -= OnVideoAdWatched;
    }
}

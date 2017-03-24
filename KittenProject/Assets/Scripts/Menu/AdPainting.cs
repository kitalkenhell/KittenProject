using UnityEngine;
using System.Collections;
using System;

public class AdPainting : MonoBehaviour
{
    const int sadDogIntervalHours = 12;

    public GameObject sadPainting;
    public GameObject happyPainting;
    public GameObject normalPainting;
    public GemsGiftScreen gemsGiftScreen;

    public bool SetDogIsSad()
    {
        DateTime nextDogIsSad = AdManager.LastTimeVideoAdWatched.AddHours(sadDogIntervalHours);

        if (nextDogIsSad < DateTime.Now)
        {
            sadPainting.SetActive(true);
            happyPainting.SetActive(false);
            normalPainting.SetActive(false);

            PostOffice.videoAdWatched -= OnVideoAdWatched;
            PostOffice.videoAdWatched += OnVideoAdWatched;

            return true;
        }
        else
        {
            sadPainting.SetActive(false);
            happyPainting.SetActive(false);
            normalPainting.SetActive(true);

            return false;
        }
    }

    void UnregisterVideoAdEvent()
    {
        PostOffice.videoAdWatched -= OnVideoAdWatched;
        PostOffice.videoAdWatched -= OnVideoAdClickedAndWatched;
    }

    void OnDestroy()
    {
        UnregisterVideoAdEvent();
    }

    void OnVideoAdWatched()
    {
        sadPainting.SetActive(false);
        happyPainting.SetActive(true);
        UnregisterVideoAdEvent();
    }

    public void OnVideoAdClickedAndWatched()
    {
        PostOffice.videoAdWatched -= OnVideoAdClickedAndWatched;

        gemsGiftScreen.Reward = AdManager.defaultVideoAdReward;
        gemsGiftScreen.gameObject.SetActive(true);

        PostOffice.videoAdWatched -= OnVideoAdWatched;
    }

    public void OnWatchAdButtonClicked()
    {
        PostOffice.videoAdWatched -= OnVideoAdClickedAndWatched;
        PostOffice.videoAdWatched += OnVideoAdClickedAndWatched;
        AdManager.Instance.ShowVideoAd();
    }
}

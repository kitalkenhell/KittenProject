using UnityEngine;
using System.Collections;
using System;

public class AdPainting : MonoBehaviour
{
    const int sadDogIntervalHours = 12;

    public GameObject sadPainting;
    public GameObject happyPainting;
    public GameObject normalPainting;

    bool videoAdEventRegistered = false;

    public bool SetDogIsSad()
    {
        DateTime nextDogIsSad = AdManager.LastTimeVideoAdWatched.AddHours(sadDogIntervalHours);

        if (nextDogIsSad < DateTime.Now)
        {
            sadPainting.SetActive(true);
            happyPainting.SetActive(false);
            normalPainting.SetActive(false);

            if (!videoAdEventRegistered)
            {
                PostOffice.videoAdWatched += OnVideoAdWatched;
                videoAdEventRegistered = true; 
            }

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
        if (videoAdEventRegistered)
        {
            videoAdEventRegistered = false;
            PostOffice.videoAdWatched -= OnVideoAdWatched;
        }
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

    public void OnWatchAdButtonClicked()
    {
        AdManager.Instance.ShowVideoAd();
    }
}

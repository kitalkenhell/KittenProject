using UnityEngine;
using System.Collections;
using System;

public class AdManager : MonoBehaviour
{
    public const int defaultVideoAdReward = 200;
    public const int coinsMultiplier = 2;

    const string dateFormat = "yyyy-MM-dd HH:mm:ss";
    const int eventsNeededToShowAd = 6;
    const int eventCounterStartingOffset = 1;
    const int FirstTimeVideoAdWatchedOffset = -11;
    const string toastText = "{0} Gems earned!";

    static int eventCounter;

    AdMobManager adMobManager;
    ChartboostManager chartboostManager;

    int currentAdReward = defaultVideoAdReward;

    public static DateTime LastTimeVideoAdWatched
    {
        get
        {
            if (PlayerPrefs.HasKey("LastTimeVideoAdWatched"))
            {
                return DateTime.ParseExact(PlayerPrefs.GetString("LastTimeVideoAdWatched"), dateFormat, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                PlayerPrefs.SetString("LastTimeVideoAdWatched", DateTime.Now.Add(new TimeSpan(FirstTimeVideoAdWatchedOffset, 0, 0)).ToString(dateFormat));
                return DateTime.Now;
            }
        }

        set
        {
            PlayerPrefs.SetString("LastTimeVideoAdWatched", value.ToString(dateFormat));
        }
    }

    public static AdManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = typeof(AdManager).ToString();

                return gameObject.AddComponent<AdManager>();
            }

            return instance;
        }
    }

    static AdManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            eventCounter = eventCounterStartingOffset;

            DontDestroyOnLoad(gameObject);

            adMobManager = gameObject.AddComponent<AdMobManager>();
            chartboostManager = gameObject.AddComponent<ChartboostManager>();

            chartboostManager.Initialize();

            adMobManager.RequestInterstitial();
            chartboostManager.RequestInterstitial();
            chartboostManager.RequestVideoAd();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementEventCounter()
    {
        ++eventCounter;

        if (eventCounter >= eventsNeededToShowAd)
        {
            if (UnityAdManager.IsInterstitialLoaded())
            {
                eventCounter = 0;
                UnityAdManager.ShowInterstitial();
            }
            else if (chartboostManager.IsInterstitialLoaded())
            {
                eventCounter = 0;
                chartboostManager.ShowInterstitial();
            }
            else if (adMobManager.IsInterstitialLoaded())
            {
                eventCounter = 0;
                adMobManager.ShowInterstitial(); 
            }
        }
    }

    public bool IsVideoAdAvailable
    {
        get
        {
            return UnityAdManager.IsVideoAdLoaded() || chartboostManager.IsVideoAdLoaded();
        }
    }

    public void ShowVideoAd(int reward = defaultVideoAdReward)
    {
        currentAdReward = reward;

        if (UnityAdManager.IsVideoAdLoaded())
        {
            UnityAdManager.ShowVideoAd();
        }
        else if (chartboostManager.IsVideoAdLoaded())
        {
            chartboostManager.ShowVideoAd();
        }
        else
        {
            Android.ShowToast(Strings.Ad.noAd);
        }

#if UNITY_EDITOR
        AdManager.LastTimeVideoAdWatched = DateTime.Now;
        PostOffice.PostVideoAdWatched();
#endif
    }

    public void OnVideoAdWatched()
    {
        AdManager.LastTimeVideoAdWatched = DateTime.Now;
        PostOffice.PostVideoAdWatched();
        PersistentData.Coins += currentAdReward;
        Android.ShowToast(toastText);
    }

}

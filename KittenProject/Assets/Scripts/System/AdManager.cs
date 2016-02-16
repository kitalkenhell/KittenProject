using UnityEngine;
using System.Collections;

public class AdManager : MonoBehaviour
{
    const int eventsNeededToShowAd = 4;
    const int eventCounterStartingOffset = 1;

    static int eventCounter;

    AdMobManager adMobManager;
    ChartboostManager chartboostManager;

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
            if (chartboostManager.IsInterstitialLoaded())
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

}

using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{

#if UNITY_EDITOR
    const string bannerId = "unused";
    const string interstitialId = "unused";
#elif UNITY_ANDROID
    const string bannerId = "ca-app-pub-2026328596529981/9822243153";
    const string interstitialId = "ca-app-pub-2026328596529981/1101444759";
#endif

    const float retryRequestDelay = 15;
    const int eventsNeededToShowAd = 5;
    const int eventCounterStartingOffset = 2;

    public bool ShowDebugUi = false;

    public static AdMobManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = typeof(AdMobManager).ToString();

                return gameObject.AddComponent<AdMobManager>();
            }

            return instance;
        }
    }

    static AdMobManager instance;

    static BannerView banner;
    static InterstitialAd interstitial;

    static int eventCounter;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            eventCounter = eventCounterStartingOffset;

            DontDestroyOnLoad(gameObject);
            RequestInterstitial();
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
            ShowInterstitial();
        }
    }

    void RequestBanner()
    {

        banner = new BannerView(bannerId, AdSize.SmartBanner, AdPosition.Top);

        banner.AdLoaded += HandleBannerLoaded;
        banner.AdFailedToLoad += HandleBannerFailedToLoad;
        banner.AdOpened += HandleBannerOpened;
        banner.AdClosing += HandleBannerClosing;
        banner.AdClosed += HandleBannerClosed;
        banner.AdLeftApplication += HandleBannerLeftApp;

        banner.LoadAd(createAdRequest());
    }

    void RequestInterstitial()
    {
        interstitial = new InterstitialAd(interstitialId);

        interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApp;

        interstitial.LoadAd(createAdRequest()); 
    }

    AdRequest createAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            eventCounter = 0;
            interstitial.Show();
        }
    }

    //Banner callback handlers
    public void HandleBannerLoaded(object sender, EventArgs args)
    {

    }

    public void HandleBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Banner failed to load with message: " + args.Message);

        StartCoroutine(RetryBannerRequest());
    }

    public void HandleBannerOpened(object sender, EventArgs args)
    {

    }

    void HandleBannerClosing(object sender, EventArgs args)
    {

    }

    public void HandleBannerClosed(object sender, EventArgs args)
    {

    }

    public void HandleBannerLeftApp(object sender, EventArgs args)
    {

    }

    //Interstitial callback handlers
    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {

    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Interstitial failed to load with message: " + args.Message);

        StartCoroutine(RetryInterestialRequest());
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {

    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {

    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApp(object sender, EventArgs args)
    {

    }

    IEnumerator RetryInterestialRequest()
    {
        yield return new WaitForSeconds(retryRequestDelay);
        RequestInterstitial();
    }

    IEnumerator RetryBannerRequest()
    {
        yield return new WaitForSeconds(retryRequestDelay);
        RequestBanner();
    }
}
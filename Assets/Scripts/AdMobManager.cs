using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    InterstitialAd interstitial;
    bool visible = false;

    void Start()
    {
        //BannerView bannerView = new BannerView("ca-app-pub-2026328596529981/9822243153", AdSize.Banner, AdPosition.Top);
        //AdRequest request = new AdRequest.Builder().Build();
        //bannerView.LoadAd(request);

        interstitial = new InterstitialAd("ca-app-pub-2026328596529981/1101444759");
        AdRequest iRequest = new AdRequest.Builder().Build();
        interstitial.LoadAd(iRequest);

        visible = false;
    }

    void Update()
    {
        if (interstitial.IsLoaded() && !visible)
        {
            interstitial.Show();
            visible = true;
        }
    }
}

﻿using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;

    void OnGUI()
    {
        const float buttonFontSize = 0.03f;
        const float leftMargin = 0.82f;
        const float rightMargin = 0.025f;
        const float buttonWidth = 1 - leftMargin - rightMargin;
        const float buttonHeight = 0.05f;
        const float buttonSpacing = 0.075f;

        float buttonY = 0.2f;

        GUI.skin.button.fontSize = (int)(buttonFontSize * Screen.height);

        Rect requestBannerRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(requestBannerRect, "Request Banner"))
        {
            RequestBanner();
        }

        buttonY += buttonSpacing;
        Rect showBannerRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(showBannerRect, "Show Banner"))
        {
            bannerView.Show();
        }

        buttonY += buttonSpacing;
        Rect hideBannerRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(hideBannerRect, "Hide Banner"))
        {
            bannerView.Hide();
        }

        buttonY += buttonSpacing;
        Rect destroyBannerRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(destroyBannerRect, "Destroy Banner"))
        {
            bannerView.Destroy();
        }

        buttonY += buttonSpacing;
        Rect requestInterstitialRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(requestInterstitialRect, "Request Interstitial"))
        {
            RequestInterstitial();
        }

        buttonY += buttonSpacing;
        Rect showInterstitialRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(showInterstitialRect, "Show Interstitial"))
        {
            ShowInterstitial();
        }

        buttonY += buttonSpacing;
        Rect destroyInterstitialRect = new Rect(leftMargin * Screen.width, buttonY * Screen.height, buttonWidth * Screen.width, buttonHeight * Screen.height);
        if (GUI.Button(destroyInterstitialRect, "Destroy Interstitial"))
        {
            interstitial.Destroy();
        }
    }

    private void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-2026328596529981/9822243153";
#elif UNITY_IPHONE
            string adUnitId = "";
#endif

        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        bannerView.AdLoaded += HandleAdLoaded;
        bannerView.AdFailedToLoad += HandleAdFailedToLoad;
        bannerView.AdOpened += HandleAdOpened;
        bannerView.AdClosing += HandleAdClosing;
        bannerView.AdClosed += HandleAdClosed;
        bannerView.AdLeftApplication += HandleAdLeftApplication;

        bannerView.LoadAd(createAdRequest());
    }

    private void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-2026328596529981/1101444759";
#elif UNITY_IPHONE
            string adUnitId = "";
#endif


        interstitial = new InterstitialAd(adUnitId);

        interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApplication;

        interstitial.LoadAd(createAdRequest());
    }

    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            .SetGender(Gender.Male)
            .SetBirthday(new DateTime(1985, 1, 1))
            .TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }

    private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("Interstitial is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}
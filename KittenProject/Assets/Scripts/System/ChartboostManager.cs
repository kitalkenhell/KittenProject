using System;
using System.Text;
using UnityEngine;
using ChartboostSDK;
using System.Collections.Generic;

public class ChartboostManager : MonoBehaviour
{
    bool ageGate = false;
    bool autocache = true;
    bool showInterstitial = true;
    bool showMoreApps = true;
    bool showRewardedVideo = true;

    public bool IsInterstitialLoaded()
    {
        return Chartboost.hasInterstitial(CBLocation.Default);
    }

    public void RequestInterstitial()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
    }

    public void ShowInterstitial()
    {
        if (Chartboost.hasInterstitial(CBLocation.Default))
        {
            Chartboost.showInterstitial(CBLocation.Default);
        }
    }

    public void Initialize()
    {
		Chartboost.CreateWithAppId("56c32d69da152775b9d212de", "5b10a7ae84a5bfd0d989c988a59f7de3616ad579");

        Chartboost.setShouldPauseClickForConfirmation(ageGate);
        Chartboost.setAutoCacheAds(autocache);

        Chartboost.didInitialize += didInitialize;
        Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
        Chartboost.didDismissInterstitial += didDismissInterstitial;
        Chartboost.didCloseInterstitial += didCloseInterstitial;
        Chartboost.didClickInterstitial += didClickInterstitial;
        Chartboost.didCacheInterstitial += didCacheInterstitial;
        Chartboost.shouldDisplayInterstitial += shouldDisplayInterstitial;
        Chartboost.didDisplayInterstitial += didDisplayInterstitial;
        Chartboost.didFailToLoadMoreApps += didFailToLoadMoreApps;
        Chartboost.didDismissMoreApps += didDismissMoreApps;
        Chartboost.didCloseMoreApps += didCloseMoreApps;
        Chartboost.didClickMoreApps += didClickMoreApps;
        Chartboost.didCacheMoreApps += didCacheMoreApps;
        Chartboost.shouldDisplayMoreApps += shouldDisplayMoreApps;
        Chartboost.didDisplayMoreApps += didDisplayMoreApps;
        Chartboost.didFailToRecordClick += didFailToRecordClick;
        Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
        Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
        Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
        Chartboost.didClickRewardedVideo += didClickRewardedVideo;
        Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
        Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
        Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
        Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;
        Chartboost.didCacheInPlay += didCacheInPlay;
        Chartboost.didFailToLoadInPlay += didFailToLoadInPlay;
        Chartboost.didPauseClickForConfirmation += didPauseClickForConfirmation;
        Chartboost.willDisplayVideo += willDisplayVideo;
    }

    void OnDestroy()
    {
        Chartboost.didInitialize -= didInitialize;
        Chartboost.didFailToLoadInterstitial -= didFailToLoadInterstitial;
        Chartboost.didDismissInterstitial -= didDismissInterstitial;
        Chartboost.didCloseInterstitial -= didCloseInterstitial;
        Chartboost.didClickInterstitial -= didClickInterstitial;
        Chartboost.didCacheInterstitial -= didCacheInterstitial;
        Chartboost.shouldDisplayInterstitial -= shouldDisplayInterstitial;
        Chartboost.didDisplayInterstitial -= didDisplayInterstitial;
        Chartboost.didFailToLoadMoreApps -= didFailToLoadMoreApps;
        Chartboost.didDismissMoreApps -= didDismissMoreApps;
        Chartboost.didCloseMoreApps -= didCloseMoreApps;
        Chartboost.didClickMoreApps -= didClickMoreApps;
        Chartboost.didCacheMoreApps -= didCacheMoreApps;
        Chartboost.shouldDisplayMoreApps -= shouldDisplayMoreApps;
        Chartboost.didDisplayMoreApps -= didDisplayMoreApps;
        Chartboost.didFailToRecordClick -= didFailToRecordClick;
        Chartboost.didFailToLoadRewardedVideo -= didFailToLoadRewardedVideo;
        Chartboost.didDismissRewardedVideo -= didDismissRewardedVideo;
        Chartboost.didCloseRewardedVideo -= didCloseRewardedVideo;
        Chartboost.didClickRewardedVideo -= didClickRewardedVideo;
        Chartboost.didCacheRewardedVideo -= didCacheRewardedVideo;
        Chartboost.shouldDisplayRewardedVideo -= shouldDisplayRewardedVideo;
        Chartboost.didCompleteRewardedVideo -= didCompleteRewardedVideo;
        Chartboost.didDisplayRewardedVideo -= didDisplayRewardedVideo;
        Chartboost.didCacheInPlay -= didCacheInPlay;
        Chartboost.didFailToLoadInPlay -= didFailToLoadInPlay;
        Chartboost.didPauseClickForConfirmation -= didPauseClickForConfirmation;
        Chartboost.willDisplayVideo -= willDisplayVideo;
    }

    void didInitialize(bool status)
    {

    }

    void didFailToLoadInterstitial(CBLocation location, CBImpressionError error)
    {

    }

    void didDismissInterstitial(CBLocation location)
    {

    }

    void didCloseInterstitial(CBLocation location)
    {

    }

    void didClickInterstitial(CBLocation location)
    {

    }

    void didCacheInterstitial(CBLocation location)
    {

    }

    bool shouldDisplayInterstitial(CBLocation location)
    {
        return showInterstitial;
    }

    void didDisplayInterstitial(CBLocation location)
    {

    }

    void didFailToLoadMoreApps(CBLocation location, CBImpressionError error)
    {

    }

    void didDismissMoreApps(CBLocation location)
    {

    }

    void didCloseMoreApps(CBLocation location)
    {

    }

    void didClickMoreApps(CBLocation location)
    {

    }

    void didCacheMoreApps(CBLocation location)
    {

    }

    bool shouldDisplayMoreApps(CBLocation location)
    {

        return showMoreApps;
    }

    void didDisplayMoreApps(CBLocation location)
    {

    }

    void didFailToRecordClick(CBLocation location, CBClickError error)
    {

    }

    void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
    {

    }

    void didDismissRewardedVideo(CBLocation location)
    {

    }

    void didCloseRewardedVideo(CBLocation location)
    {

    }

    void didClickRewardedVideo(CBLocation location)
    {

    }

    void didCacheRewardedVideo(CBLocation location)
    {

    }

    bool shouldDisplayRewardedVideo(CBLocation location)
    {
        return showRewardedVideo;
    }

    void didCompleteRewardedVideo(CBLocation location, int reward)
    {
    }

    void didDisplayRewardedVideo(CBLocation location)
    {

    }

    void didCacheInPlay(CBLocation location)
    {

    }

    void didFailToLoadInPlay(CBLocation location, CBImpressionError error)
    {

    }

    void didPauseClickForConfirmation()
    {

    }

    void willDisplayVideo(CBLocation location)
    {

    }

}



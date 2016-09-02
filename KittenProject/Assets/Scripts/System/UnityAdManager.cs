using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdManager : MonoBehaviour
{
    const string videoAdZone = "rewardedVideo";

    public static bool IsInterstitialLoaded()
    {
        return Advertisement.isInitialized && Advertisement.IsReady();
    }

    public static void ShowInterstitial()
    {
        if (IsInterstitialLoaded())
        {
            Advertisement.Show(); 
        }
    }

    public static bool IsVideoAdLoaded()
    {
        return Advertisement.isInitialized && Advertisement.IsReady(videoAdZone);
    }

    public static void ShowVideoAd()
    {
        if (IsVideoAdLoaded())
        {
            ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(videoAdZone, options);
        }
    }

    static void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            AdManager.Instance.OnVideoAdWatched();
        }
    }
}
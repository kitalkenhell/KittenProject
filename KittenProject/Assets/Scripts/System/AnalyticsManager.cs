using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public static class AnalyticsManager
{
    public static void OnLevelStarted(string sceneName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "level", sceneName },
        };

        Analytics.CustomEvent("LevelCompleted", data);
    }

    public static void OnLevelCompleted(string sceneName, float time)
    {
        Dictionary<string, object> data = new Dictionary< string, object>
        {
            { "level", sceneName },
            { "gameTime", time },
        };

        Analytics.CustomEvent("LevelCompleted", data);
    }

    public static void OnLevelFailed(string sceneName, float time, Vector3 position, bool killedByHellfire)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "level", sceneName },
            { "gameTime", time },
            { "position", position.ToString() },
            { "killedByHellfire", killedByHellfire.ToString() },
        };

        Analytics.CustomEvent("LevelFailed", data);
    }

    public static void OnLevelAbandoned(string sceneName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "level", sceneName },
        };

        Analytics.CustomEvent("LevelAbandoned", data);
    }

    public static void OnOutfitSkinBought(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("OutfitSkinBought", data);
    }

    public static void OnHatSkinBought(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("HatSkinBought", data);
    }

    public static void OnParachuteSkinBought(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("ParachuteSkinBought", data);
    }

    public static void OnUpgradeBought(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("UpgradeBought", data);
    }

    public static void OnOutfitSkinSelected(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("OutfitSkinSelected", data);
    }

    public static void OnHatSkinSelected(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("HatSkinSelected", data);
    }

    public static void OnParachuteSkinSelected(string skinName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "skinName", skinName },
        };

        Analytics.CustomEvent("ParachuteSkinSelected", data);
    }
}

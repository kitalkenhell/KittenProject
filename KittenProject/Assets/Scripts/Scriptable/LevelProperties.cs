using UnityEngine;
using System.Collections;
using System;

public class LevelProperties : ScriptableObject
{
    const string isLockedKey = "IsLocked";

    public string levelName;
    public string sceneName;
    public LevelProperties nextLevel;
    public string timeLeaderboardId;
    public float timeToGetStar;
    public int coinsToGetStar;

    public bool IsLocked
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(sceneName + "isLockedKey", 1));
        }

        set
        {
            PlayerPrefs.SetInt(sceneName + "isLockedKey", Convert.ToInt32(value));
            PlayerPrefs.Save();
        }
    }

    public bool IsCompleted
    {
        get
        {
            return BestCoinsScore > 0;
        }
    }

    public float BestTimeScore
    {
        get
        {
            return PlayerPrefs.GetFloat(sceneName + "bestTimeScore", Mathf.Infinity);
        }

        set
        {
            PlayerPrefs.SetFloat(sceneName + "bestTimeScore", value);
            PlayerPrefs.Save();
        }
    }

    public int BestCoinsScore
    {
        get
        {
            return PlayerPrefs.GetInt(sceneName + "bestCoinsScore", 0);
        }

        set
        {
            PlayerPrefs.SetInt(sceneName + "bestCoinsScore", value);
            PlayerPrefs.Save();
        }
    }

    public bool HasTimeStar
    {
        get
        {
            return BestTimeScore <= timeToGetStar;
        }
    }

    public bool HasCoinStar
    {
        get
        {
            return BestCoinsScore >= coinsToGetStar;
        }
    }

    public bool HasGoldenKittenStar
    {
        get
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(sceneName + "hasGoldenKittenStar", 0));
        }
        set
        {
            PlayerPrefs.SetInt(sceneName + "hasGoldenKittenStar", Convert.ToInt32(value));
            PlayerPrefs.Save();
        }
    }
}

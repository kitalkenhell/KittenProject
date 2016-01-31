using UnityEngine;
using System.Collections;
using System;

public class LevelProperties : ScriptableObject
{
    const string isLockedKey = "IsLocked";

    public string sceneName;
    public LevelProperties nextLevel;
    public string timeLeaderboardId;

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
}

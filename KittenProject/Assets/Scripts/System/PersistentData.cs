using UnityEngine;
using System.Collections;

public static class PersistentData
{
    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("Coins", 0);
        }

        set
        {
            PlayerPrefs.SetInt("Coins", value);
            PlayerPrefs.Save();

            PostOffice.PostAmountOfCoinsChanged();
        }
    }

    public static int MaxPlayerHealth
    {
        get
        {
            return PlayerPrefs.GetInt("GameSettingsMaxPlayerHealth", 3);
        }

        set
        {
            PlayerPrefs.SetInt("GameSettingsMaxPlayerHealth", value);
            PlayerPrefs.Save();
        }
    }

    public static string LeaderboardScoresToPost
    {
        get
        {
            return PlayerPrefs.GetString("LeaderboardScoresToPost", "");
        }
        set
        {
            PlayerPrefs.SetString("LeaderboardScoresToPost", value);
        }
    }


    public static bool HasWatchedIntroCutscene
    {
        get
        {
            return PlayerPrefs.HasKey("HasWatchedIntroCutscene");
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("HasWatchedIntroCutscene", 0);
            }
            else
            {
                PlayerPrefs.DeleteKey("HasWatchedIntroCutscene");
            }
        }
    }

    public static bool HasWatchedPlotTwistCutscene
    {
        get
        {
            return PlayerPrefs.HasKey("HasWatchedPlotTwistCutscene");
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("HasWatchedPlotTwistCutscene", 0);
            }
            else
            {
                PlayerPrefs.DeleteKey("HasWatchedPlotTwistCutscene");
            }
        }
    }

    public static bool HasWatchedBossFightCutscene
    {
        get
        {
            return PlayerPrefs.HasKey("HasWatchedBossFightCutscene");
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("HasWatchedBossFightCutscene", 0);
            }
            else
            {
                PlayerPrefs.DeleteKey("HasWatchedBossFightCutscene");
            }
        }
    }

    public static bool HasWatchedEndingCutscene
    {
        get
        {
            return PlayerPrefs.HasKey("HasWatchedEndingCutscene");
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("HasWatchedEndingCutscene", 0);
            }
            else
            {
                PlayerPrefs.DeleteKey("HasWatchedEndingCutscene");
            }
        }
    }

    public static bool IsHavingBodySkin(string name)
    {
        return PlayerPrefs.HasKey("IsHavingBodySkin" + name);
    }

    public static void IsHavingBodySkin(string name, bool newValue)
    {
        if (newValue)
        {
            PlayerPrefs.SetInt("IsHavingBodySkin" + name, 0);
        }
        else
        {
            PlayerPrefs.DeleteKey("IsHavingBodySkin" + name);
        }
    }

    public static bool IsHavingParachuteSkin(string name)
    {
        return PlayerPrefs.HasKey("IsHavingParachuteSkin" + name);
    }

    public static void IsHavingParachuteSkin(string name, bool newValue)
    {
        if (newValue)
        {
            PlayerPrefs.SetInt("IsHavingParachuteSkin" + name, 0);
        }
        else
        {
            PlayerPrefs.DeleteKey("IsHavingParachuteSkin" + name);
        }
    }

    public static bool IsHavingHatSkin(string name)
    {
        return PlayerPrefs.HasKey("IsHavingHatSkin" + name);
    }

    public static void IsHavingHatSkin(string name, bool newValue)
    {
        if (newValue)
        {
            PlayerPrefs.SetInt("IsHavingHatSkin" + name, 0);
        }
        else
        {
            PlayerPrefs.DeleteKey("IsHavingHatSkin" + name);
        }
    }

    public static bool IsHavingUpgrade(string name)
    {
        return PlayerPrefs.HasKey("IsHavingUpgrade" + name);
    }

    public static void IsHavingUpgrade(string name, bool newValue)
    {
        if (newValue)
        {
            PlayerPrefs.SetInt("IsHavingUpgrade" + name, 0);
        }
        else
        {
            PlayerPrefs.DeleteKey("IsHavingUpgrade" + name);
        }
    }
}

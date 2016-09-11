using UnityEngine;
using System.Collections;

public static class GameSettings 
{
    public static string PlayerBodySkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerBodySkin", "Classic");
        }

        set
        {
            AnalyticsManager.OnOutfitSkinSelected(value);
            PlayerPrefs.SetString("GameSettingsPlayerBodySkin", value);
            PlayerPrefs.Save();
        }
    }

    public static string PlayerHatSkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerHatSkin", "Classic");
        }

        set
        {
            AnalyticsManager.OnHatSkinSelected(value);
            PlayerPrefs.SetString("GameSettingsPlayerHatSkin", value);
            PlayerPrefs.Save();
        }
    }

    public static string PlayerParachuteSkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerParachuteSkin", "Classic");
        }

        set
        {
            AnalyticsManager.OnParachuteSkinSelected(value);
            PlayerPrefs.SetString("GameSettingsPlayerParachuteSkin", value);
            PlayerPrefs.Save();
        }
    }

    public static float UiScale
    {
        get
        {
            return PlayerPrefs.GetFloat("GameSettingsUiScale", 1.2f);
        }

        set
        {
            PlayerPrefs.SetFloat("GameSettingsUiScale", value);
        }
    }
}

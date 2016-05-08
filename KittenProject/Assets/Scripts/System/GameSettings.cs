using UnityEngine;
using System.Collections;

public static class GameSettings 
{
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

    public static string PlayerBodySkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerBodySkin", "");
        }

        set
        {
            PlayerPrefs.SetString("GameSettingsPlayerBodySkin", value);
            PlayerPrefs.Save();
        }
    }

    public static string PlayerHatSkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerHatSkin", "");
        }

        set
        {
            PlayerPrefs.SetString("GameSettingsPlayerHatSkin", value);
            PlayerPrefs.Save();
        }
    }

    public static string PlayerParachuteSkin
    {
        get
        {
            return PlayerPrefs.GetString("GameSettingsPlayerParachuteSkin", "");
        }

        set
        {
            PlayerPrefs.SetString("GameSettingsPlayerParachuteSkin", value);
            PlayerPrefs.Save();
        }
    }

    public static float UiScale
    {
        get
        {
            return PlayerPrefs.GetFloat("GameSettingsUiScale", 1);
        }

        set
        {
            PlayerPrefs.SetFloat("GameSettingsUiScale", value);
        }
    }
}

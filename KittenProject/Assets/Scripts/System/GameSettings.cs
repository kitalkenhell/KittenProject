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

    public static int PlayerBodySkinIndex
    {
        get
        {
            return PlayerPrefs.GetInt("GameSettingsPlayerBodySkinIndex", 0);
        }

        set
        {
            PlayerPrefs.SetInt("GameSettingsPlayerBodySkinIndex", value);
            PlayerPrefs.Save();
        }
    }

    public static int PlayerHatSkinIndex
    {
        get
        {
            return PlayerPrefs.GetInt("GameSettingsPlayerHatSkinIndex", 0);
        }

        set
        {
            PlayerPrefs.SetInt("GameSettingsPlayerHatSkinIndex", value);
            PlayerPrefs.Save();
        }
    }

    public static int PlayerParachuteSkinIndex
    {
        get
        {
            return PlayerPrefs.GetInt("GameSettingsPlayerParachuteSkinIndex", 0);
        }

        set
        {
            PlayerPrefs.SetInt("GameSettingsPlayerParachuteSkinIndex", value);
            PlayerPrefs.Save();
        }
    }
}

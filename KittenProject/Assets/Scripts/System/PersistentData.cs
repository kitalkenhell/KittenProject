using UnityEngine;
using System.Collections;

public static class PersistentData
{
    public static int Coins
    {
        get
        {
            return PlayerPrefs.GetInt("Coins", 500);
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

    public static bool IsHavingBodySkin(string name)
    {
        return PlayerPrefs.HasKey("IsHavingBodySkin" + name);
    }

    public static void IsHavingBodySkin(string name, bool newValue)
    {
        if (newValue == true)
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
        if (newValue == true)
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
        if (newValue == true)
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
        if (newValue == true)
        {
            PlayerPrefs.SetInt("IsHavingUpgrade" + name, 0);
        }
        else
        {
            PlayerPrefs.DeleteKey("IsHavingUpgrade" + name);
        }
    }
}

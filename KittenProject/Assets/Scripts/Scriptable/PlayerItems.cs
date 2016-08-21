using UnityEngine;
using System.Collections;
using System;

public class PlayerItems : ScriptableObject
{
    public PlayerBodySkin[] bodySkins;
    public PlayerHatSkin[] hatSkins;
    public PlayerParachuteSkin[] parachuteSkins;
    public PlayerUpgrade[] upgrades;

    public PlayerBodySkin GetBodySkin(string name)
    {
        foreach (var skin in bodySkins)
        {
            if (skin.itemName == name)
            {
                return skin;
            }
        }

        return bodySkins.First();
    }

    public PlayerHatSkin GetHatSkin(string name)
    {
        foreach (var skin in hatSkins)
        {
            if (skin.itemName == name)
            {
                return skin;
            }
        }

        return hatSkins.First();
    }

    public PlayerParachuteSkin GetParachuteSkin(string name)
    {
        foreach (var skin in parachuteSkins)
        {
            if (skin.itemName == name)
            {
                return skin;
            }
        }

        return parachuteSkins.First();
    }

    public PlayerBodySkin GetEquipedBodySkin()
    {
        return GetBodySkin(GameSettings.PlayerBodySkin);
    }

    public PlayerHatSkin GetEquipedHatSkin()
    {
        return GetHatSkin(GameSettings.PlayerHatSkin);
    }

    public PlayerParachuteSkin GetEquipedParachuteSkin()
    {
        return GetParachuteSkin(GameSettings.PlayerParachuteSkin);
    }

    public bool HasAllSkins()
    {
        return HasAllBodySkins() && HasAllHats() && HasAllParachutes();
    }

    public bool HasAllBodySkins()
    {
        foreach (var skin in bodySkins)
        {
            if (!PersistentData.IsHavingBodySkin(skin.itemName))
            {
                return false;
            }
        }

        return true;
    }

    public bool HasAllHats()
    {
        foreach (var skin in hatSkins)
        {
            if (!PersistentData.IsHavingBodySkin(skin.itemName))
            {
                return false;
            }
        }

        return true;
    }

    public bool HasAllParachutes()
    {
        foreach (var skin in parachuteSkins)
        {
            if (!PersistentData.IsHavingBodySkin(skin.itemName))
            {
                return false;
            }
        }

        return true;
    }

    public PlayerBodySkin GetRandomNotUnlockedBodySkin()
    {
        int index = UnityEngine.Random.Range(0, bodySkins.Length);

        if (PersistentData.IsHavingBodySkin(bodySkins[index].itemName))
        {
            for (int i = index + 1; i < bodySkins.Length; ++i)
            {
                if (!PersistentData.IsHavingBodySkin(bodySkins[i].itemName))
                {
                    return bodySkins[i];
                }
            }

            for (int i = index - 1; i >= 0; --i)
            {
                if (!PersistentData.IsHavingBodySkin(bodySkins[i].itemName))
                {
                    return bodySkins[i];
                }
            }
        }
        else
        {
            return bodySkins[index];
        }

        return null;
    }

    public PlayerHatSkin GetRandomNotUnlockedHat()
    {
        int index = UnityEngine.Random.Range(0, hatSkins.Length);

        if (PersistentData.IsHavingHatSkin(hatSkins[index].itemName))
        {
            for (int i = index + 1; i < hatSkins.Length; ++i)
            {
                if (!PersistentData.IsHavingHatSkin(hatSkins[i].itemName))
                {
                    return hatSkins[i];
                }
            }

            for (int i = index - 1; i >= 0; --i)
            {
                if (!PersistentData.IsHavingHatSkin(hatSkins[i].itemName))
                {
                    return hatSkins[i];
                }
            }
        }
        else
        {
            return hatSkins[index];
        }

        return null;
    }

    public PlayerParachuteSkin GetRandomNotUnlockedParachute()
    {
        int index = UnityEngine.Random.Range(0, parachuteSkins.Length);

        if (PersistentData.IsHavingParachuteSkin(parachuteSkins[index].itemName))
        {
            for (int i = index + 1; i < parachuteSkins.Length; ++i)
            {
                if (!PersistentData.IsHavingParachuteSkin(parachuteSkins[i].itemName))
                {
                    return parachuteSkins[i];
                }
            }

            for (int i = index - 1; i >= 0; --i)
            {
                if (!PersistentData.IsHavingParachuteSkin(parachuteSkins[i].itemName))
                {
                    return parachuteSkins[i];
                }
            }
        }
        else
        {
            return parachuteSkins[index];
        }

        return null;
    }
}

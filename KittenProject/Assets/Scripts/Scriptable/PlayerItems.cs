﻿using UnityEngine;
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
}
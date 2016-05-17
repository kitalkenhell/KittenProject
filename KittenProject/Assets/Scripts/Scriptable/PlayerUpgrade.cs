using UnityEngine;
using System.Collections;
using System;

public abstract class PlayerUpgrade : PlayerItem
{
    public string icon;
    public string description;
    public PlayerUpgrade baseUpgrade;

    public abstract void OnBought();

    public bool Locked
    {
        get
        {
            return !(baseUpgrade == null || PersistentData.IsHavingUpgrade(baseUpgrade.itemName));
        }
    }
}
using UnityEngine;
using System.Collections;
using System;

public class PlayerMaxHealthUpgrade : PlayerUpgrade
{
    public int health = 0;

    public override void OnBought()
    {
        PersistentData.MaxPlayerHealth = health;
    }
}
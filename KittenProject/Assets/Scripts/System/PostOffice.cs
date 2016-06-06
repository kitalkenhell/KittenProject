using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class PostOffice
{
    public delegate void DebugMessage(string message);
    public delegate void CoinEvent(int amount);
    public delegate void PlayerHealthChangedEvent(int oldAmount,int currentAmount);
    public delegate void PlayerUpgradeBought(PlayerUpgrade upgrade);
    public delegate void PlayerHatSkinEvent(PlayerHatSkin skin);
    public delegate void PlayerBodySkinEvent(PlayerBodySkin skin);
    public delegate void PlayerParachuteSkinEvent(PlayerParachuteSkin skin);

    public static event DebugMessage debugMessage;
    public static event CoinEvent coinCollected;
    public static event CoinEvent coinDropped;
    public static event Action heartCollected;
    public static event Action goldenKittenCollected;
    public static event Action playedDied;
    public static event PlayerHealthChangedEvent PlayerHealthChanged;
    public static event Action levelStopwatchStarted;
    public static event Action victory;
    public static event Action levelQuit;
    public static event Action videoAdWatched;
    public static event Action amountOfCoinsChanged;
    public static event PlayerUpgradeBought playerUpgradeBought;
    public static event PlayerHatSkinEvent playerHatSkinBought;
    public static event PlayerHatSkinEvent playerHatSkinEquipped;
    public static event PlayerBodySkinEvent playerBodySkinBought;
    public static event PlayerBodySkinEvent playerBodySkinEquipped;
    public static event PlayerParachuteSkinEvent playerParachuteSkinBought;
    public static event PlayerParachuteSkinEvent playerParachuteSkinEquipped;

    public static void PostDebugMessage(object message)
    {
        if (debugMessage != null)
        {
            debugMessage(message.ToString());
        }
    }

    public static void PostCoinCollected(int amount = 1)
    {
        if (coinCollected != null)
        {
            coinCollected(amount);
        }
    }

    public static void PostCoinDropped(int amount = 1)
    {
        if (coinDropped != null)
        {
            coinDropped(amount);
        }
    }

    public static void PostHeartCollected(int amount = 1)
    {
        if (heartCollected != null)
        {
            heartCollected();
        }
    }

    public static void PostGoldenKittenCollected(int amount = 1)
    {
        if (goldenKittenCollected != null)
        {
            goldenKittenCollected();
        }
    }

    public static void PostPlayerHealthChanged(int oldAmount, int currentAmount)
    {
        if (PlayerHealthChanged != null)
        {
            PlayerHealthChanged(oldAmount, currentAmount); 
        }
    }

    public static void PostPlayerDied()
    {
        if (playedDied != null)
        {
            playedDied();
        }
    }

    public static void PostLevelStopwatchStarted()
    {
        if (levelStopwatchStarted != null)
        {
            levelStopwatchStarted();
        }
    }

    public static void PostVictory()
    {
        if (victory != null)
        {
            victory();
        }
    }

    public static void PostLevelQuit()
    {
        if (levelQuit != null)
        {
            levelQuit();
        }
    }

    public static void PostVideoAdWatched()
    {
        if (videoAdWatched != null)
        {
            videoAdWatched();
        }
    }

    public static void PostAmountOfCoinsChanged()
    {
        if (amountOfCoinsChanged != null)
        {
            amountOfCoinsChanged();
        }
    }

    public static void PostPlayerUpgradeBought(PlayerUpgrade upgrade)
    {
        if (playerUpgradeBought != null)
        {
            playerUpgradeBought(upgrade);
        }
    }

    public static void PostPlayerHatSkinBought(PlayerHatSkin skin)
    {
        if (playerHatSkinBought != null)
        {
            playerHatSkinBought(skin);
        }
    }

    public static void PostPlayerHatSkinEquipped(PlayerHatSkin skin)
    {
        if (playerHatSkinEquipped!= null)
        {
            playerHatSkinEquipped(skin);
        }
    }

    public static void PostPlayerBodySkinBought(PlayerBodySkin skin)
    {
        if (playerBodySkinBought != null)
        {
            playerBodySkinBought(skin);
        }
    }

    public static void PostPlayerBodySkinEquipped(PlayerBodySkin skin)
    {
        if (playerBodySkinEquipped != null)
        {
            playerBodySkinEquipped(skin);
        }
    }

    public static void PostPlayerParachuteSkinBought(PlayerParachuteSkin skin)
    {
        if (playerParachuteSkinBought != null)
        {
            playerParachuteSkinBought(skin);
        }
    }

    public static void PostPlayerParachuteSkinEquipped(PlayerParachuteSkin skin)
    {
        if (playerParachuteSkinEquipped != null)
        {
            playerParachuteSkinEquipped(skin);
        }
    }
}


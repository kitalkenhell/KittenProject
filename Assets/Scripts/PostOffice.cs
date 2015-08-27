using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class PostOffice
{
    public delegate void DebugMessage(string message);
    public delegate void CoinEvent(int amount);

    public static event DebugMessage debugMessage;
    public static event CoinEvent coinCollected;
    public static event CoinEvent coinDropped;

    public static void PostDebugMessage(string message)
    {
        if (debugMessage != null)
        {
            debugMessage(message);
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
}


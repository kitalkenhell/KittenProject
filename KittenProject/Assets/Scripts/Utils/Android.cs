using UnityEngine;
using System.Collections;

public static class Android
{
    static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    static AndroidJavaClass plugin = new AndroidJavaClass("com.kittenno.javautils.Utils");
    static AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

    public static void ShowToast(string message)
    {
        plugin.CallStatic("ShowToast", activity, message);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class SocialManager
{

    static bool IsLoggedIn
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }


    public static void SignIn()
    {
        if (!IsLoggedIn)
        {
            GooglePlayGames.PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(OnSignedIn); 
        }
    }

    public static void OnSignedIn(bool result)
    {
        if (result)
        {

        }
    }

    public static void ShowLeaderboard(string key)
    {
        if (IsLoggedIn)
        {
            GooglePlayGames.PlayGamesPlatform.Instance.ShowLeaderboardUI(key);
        }
        else
        {
            SignIn();
        }
    }

    public static void PostLevelTimeToLeaderboard(string key, float time)
    {
        if (IsLoggedIn)
        {
            Social.ReportScore((int) System.TimeSpan.FromSeconds(time).TotalMilliseconds, key, 
                (bool success) =>
                {
                    Debug.Log("Success wow? " + time);

                }
            );
        }
    }

    public static void ShowAchievements()
    {
        if (IsLoggedIn)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            SignIn();
        }
    }

    public static void ShowLeaderboards()
    {
        if (IsLoggedIn)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            SignIn();
        }
    }
}

using UnityEngine;
using System;
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
            PostMissingScores();
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
            Social.ReportScore((int)System.TimeSpan.FromSeconds(time).TotalMilliseconds, key,
                (bool success) =>
                {
                    if (!success)
                    {
                        PersistentData.LeaderboardScoresToPost = PersistentData.LeaderboardScoresToPost + ";" + key + "=" + time;
                    }
                }
            );
        }
        else
        {
            PersistentData.LeaderboardScoresToPost = PersistentData.LeaderboardScoresToPost + ";" + key + "=" + time;
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

    static void PostMissingScores()
    {
        string[] allScores = PersistentData.LeaderboardScoresToPost.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        PersistentData.LeaderboardScoresToPost = "";

        foreach (var score in allScores)
        {
            try
            {
                int keyIndex = 0;
                int timeIndex = 1;

                string[] keyAndValue = score.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                PostLevelTimeToLeaderboard(keyAndValue[keyIndex], float.Parse(keyAndValue[timeIndex]));
            }
            catch (Exception)
            {
                //ignore
            }
        }
    }
}

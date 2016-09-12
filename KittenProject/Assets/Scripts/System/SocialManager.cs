using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class SocialManager
{
    const int maxPrefsStringLenght = 1000;

    public class Achievements
    {
        public const string finishTutorial = "CgkIgs_k3o0ZEAIQAw"; 
        public const string jumps = "CgkIgs_k3o0ZEAIQAQ";
        public const string kidnappedKitten = "CgkIgs_k3o0ZEAIQBQ"; 
        public const string kittenSaved = "CgkIgs_k3o0ZEAIQMA"; 
        public const string gemsCollected = "CgkIgs_k3o0ZEAIQNg"; 
        public const string hatsUnlocked = "CgkIgs_k3o0ZEAIQNQ"; 
        public const string coinFliesCollected = "CgkIgs_k3o0ZEAIQNA"; 
        public const string goldenHourglasses = "CgkIgs_k3o0ZEAIQMQ"; 
        public const string goldenGems = "CgkIgs_k3o0ZEAIQMg"; 
        public const string goldenKittens = "CgkIgs_k3o0ZEAIQMw"; 
        public const string giftBought = "CgkIgs_k3o0ZEAIQDA";
        public const string runAwayFromDragon = "CgkIgs_k3o0ZEAIQAg";
    }

    static bool IsLoggedIn
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    public static void AutoSignIn()
    {
        if (!IsLoggedIn)
        {
            if (!PersistentData.TriedToSignInToSocialPlatform)
            {
                GooglePlayGames.PlayGamesPlatform.Activate();
                Social.localUser.Authenticate(OnSignedIn);
            }
            else if (PersistentData.UseSocialPlatform)
            {
                GooglePlayGames.PlayGamesPlatform.Activate();
                Social.localUser.Authenticate(OnSignedIn);
            }
        }
    }

    public static void OnSignedIn(bool result)
    {
        if (result)
        {
            PostMissingScores();
            PostMissingAchievements();
            PersistentData.UseSocialPlatform = true;
        }
        else if (PersistentData.TriedToSignInToSocialPlatform)
        {
            PersistentData.UseSocialPlatform = false;
            PersistentData.TriedToSignInToSocialPlatform = true;
        }
        PersistentData.TriedToSignInToSocialPlatform = true;
    }

    public static void UnlockAchievement(string id)
    {
        const float successProgress = 100;

        if (IsLoggedIn)
        {
            Social.ReportProgress(id, successProgress, (bool success) =>
            {
                if (!success)
                {
                    PersistentData.AchievementsToUnlock += ";" + id;
                }
            }); 
        }
        else
        {
            PersistentData.AchievementsToUnlock += ";" + id;
        }
    }

    public static void IncrementAchievement(string id, int steps = 1)
    {
        if (IsLoggedIn)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(id, steps, (bool success) =>
            {
                if (!success && PersistentData.AchievementsToIncrement.Length < maxPrefsStringLenght)
                {
                    PersistentData.AchievementsToIncrement = PersistentData.AchievementsToIncrement + ";" + id + "=" + steps;
                }
            }); 
        }
        else
        {
            if (PersistentData.AchievementsToIncrement.Length < maxPrefsStringLenght)
            {
                PersistentData.AchievementsToIncrement = PersistentData.AchievementsToIncrement + ";" + id + "=" + steps;
            }
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
                    if (!success && PersistentData.LeaderboardScoresToPost.Length < maxPrefsStringLenght)
                    {
                        PersistentData.LeaderboardScoresToPost = PersistentData.LeaderboardScoresToPost + ";" + key + "=" + time;
                    }
                }
            );
        }
        else
        {
            if (PersistentData.LeaderboardScoresToPost.Length < maxPrefsStringLenght)
            {
                PersistentData.LeaderboardScoresToPost = PersistentData.LeaderboardScoresToPost + ";" + key + "=" + time;
            }
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

    static void SignIn()
    {
        if (!IsLoggedIn)
        {
            GooglePlayGames.PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(OnSignedIn);
        }
    }

    static void PostMissingAchievements()
    {
        string[] toUnlock = PersistentData.AchievementsToUnlock.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        string[] toIncrement = PersistentData.AchievementsToIncrement.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        PersistentData.AchievementsToUnlock = "";
        PersistentData.AchievementsToIncrement = "";

        foreach (var achievement in toUnlock)
        {
            UnlockAchievement(achievement);
        }

        foreach (var achievement in toIncrement)
        {
            try
            {
                int keyIndex = 0;
                int stepsIndex = 1;

                string[] keyAndValue = achievement.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                IncrementAchievement(keyAndValue[keyIndex], int.Parse(keyAndValue[stepsIndex]));
            }
            catch (Exception)
            {
                //ignore
            }
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

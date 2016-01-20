using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class PlayGamesManager : MonoBehaviour
{

    void Start()
    {
        if (!Social.localUser.authenticated)
        {
            GooglePlayGames.PlayGamesPlatform.Activate();
            Social.localUser.Authenticate(OnSignedIn); 
        }
    }


    void OnSignedIn(bool result)
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }
}

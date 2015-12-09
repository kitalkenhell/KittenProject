using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class PlayGamesManager : MonoBehaviour
{

    void Start()
    {
        GooglePlayGames.PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(OnSignedIn);
    }


    void OnSignedIn(bool result)
    {
        PostOffice.PostDebugMessage(result.ToString());

        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }
}

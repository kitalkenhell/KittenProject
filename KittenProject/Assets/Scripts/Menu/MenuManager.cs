using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
    int playButtonPressedAnimHash;
    int backButtonPressedAnimHash;

    public Animator menuAnimator;

	void Start () 
	{
        playButtonPressedAnimHash = Animator.StringToHash("PlayButtonPressed");
        backButtonPressedAnimHash = Animator.StringToHash("BackButtonPressed");

        SocialManager.SignIn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuAnimator.SetTrigger(backButtonPressedAnimHash);
        }
    }

    public void OnPlayButtonClicked()
    {
        menuAnimator.SetTrigger(playButtonPressedAnimHash);
    }

    public void OnAchievementsButtonClicked()
    {
        SocialManager.ShowAchievements();
    }


    public void OnLeaderboardsButtonClicked()
    {
        SocialManager.ShowLeaderboards();
    }

}

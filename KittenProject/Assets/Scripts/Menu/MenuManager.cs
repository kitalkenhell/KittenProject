using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public LevelProperties firstLevel;

    [HideInInspector]
    public string LevelToLoad;

    int playButtonPressedAnimHash;
    int backButtonPressedAnimHash;
    int StartGameButtonPressedAnimHash;

    Animator menuAnimator;

	void Start () 
	{
        playButtonPressedAnimHash = Animator.StringToHash("PlayButtonPressed");
        backButtonPressedAnimHash = Animator.StringToHash("BackButtonPressed");
        StartGameButtonPressedAnimHash = Animator.StringToHash("StartGameButtonPressed");

        menuAnimator = GetComponent<Animator>();

        firstLevel.IsLocked = false;

        AdManager.Instance.IncrementEventCounter();
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

    public void OnStartGameButtonClicked()
    {
        menuAnimator.SetTrigger(StartGameButtonPressedAnimHash);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}

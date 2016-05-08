using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public LevelProperties firstLevel;
    public PlayerBodySkin firstBodySkin;
    public PlayerHatSkin firstHatSkin;
    public PlayerParachuteSkin firstParachuteSkin;

    public string LevelToLoad
    {
        get;
        set;
    }

    int playButtonPressedAnimHash;
    int backButtonPressedAnimHash;
    int startGameButtonPressedAnimHash;
    int wardrobeButtonPressedAnimHash;

    Animator menuAnimator;

	void Start() 
	{
        playButtonPressedAnimHash = Animator.StringToHash("PlayButtonPressed");
        backButtonPressedAnimHash = Animator.StringToHash("BackButtonPressed");
        startGameButtonPressedAnimHash = Animator.StringToHash("StartGameButtonPressed");
        wardrobeButtonPressedAnimHash = Animator.StringToHash("WardrobeButtonPressed");

        menuAnimator = GetComponent<Animator>();

        firstLevel.IsLocked = false;

        PersistentData.IsHavingBodySkin(firstBodySkin.skinName, true);
        PersistentData.IsHavingHatSkin(firstHatSkin.skinName, true);
        PersistentData.IsHavingParachuteSkin(firstParachuteSkin.skinName, true);

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

    public void OnWardrobeButtonClicked()
    {
        menuAnimator.SetTrigger(wardrobeButtonPressedAnimHash);
    }

    public void OnLeaderboardsButtonClicked()
    {
        SocialManager.ShowLeaderboards();
    }

    public void OnStartGameButtonClicked()
    {
        menuAnimator.SetTrigger(startGameButtonPressedAnimHash);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelToLoad);
    }
}

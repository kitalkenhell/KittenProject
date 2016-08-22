using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PauseMenu : MonoBehaviour
{
    const string gemsToGetStar = "Get {0} Gems";
    const string timeToGetStar = "Finish in {0}\"";

    public Image grayedOutGem;
    public Image grayedOutHourglass;
    public Image grayedOutKitten;
    public Image goldenGem;
    public Image goldenHourglass;
    public Image goldenKitten;
    public Text gemText;
    public Text hourglassText;
    public Text levelName;
    public LevelFlow levelFlow;
    public GameObject optionsMenu;

    int toggleAnimHash;
    int toggleContentAnimHash;

    Animator animator;

    bool visible;

    void Awake()
    {
        animator = GetComponent<Animator>();

        toggleAnimHash = Animator.StringToHash("Toggle");
        toggleContentAnimHash = Animator.StringToHash("ToggleContent");

        visible = false;
    }

    void Start()
    {
        levelName.text = CoreLevelObjects.levelProperties.levelName;
        gemText.text = String.Format(gemsToGetStar, CoreLevelObjects.levelProperties.coinsToGetStar);
        hourglassText.text = String.Format(timeToGetStar, CoreLevelObjects.levelProperties.timeToGetStar);
    }

    void OnEnable()
    {
        Time.timeScale = 0;

        grayedOutGem.enabled = !CoreLevelObjects.levelProperties.HasCoinStar;
        goldenGem.enabled = CoreLevelObjects.levelProperties.HasCoinStar;

        grayedOutHourglass.enabled = !CoreLevelObjects.levelProperties.HasTimeStar;
        goldenHourglass.enabled = CoreLevelObjects.levelProperties.HasTimeStar;

        grayedOutKitten.enabled = !CoreLevelObjects.levelProperties.HasGoldenKittenStar;
        goldenKitten.enabled = CoreLevelObjects.levelProperties.HasGoldenKittenStar;

        animator.SetTrigger(toggleAnimHash);
        visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && visible)
        {
            ResumeGame();
        }
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    public void OnRestartButtonClicked()
    {
        ResumeGame();
        levelFlow.OnRestartLevel();
    }

    public void OnMenuButtonClicked()
    {
        PostOffice.PostLevelQuit();
        ResumeGame();
    }

    public void OnOptionsButtonClicked()
    {
        optionsMenu.SetActive(true);
        animator.SetTrigger(toggleContentAnimHash);
        visible = false; 
    }

    public void BackFromOptions()
    {
        animator.SetTrigger(toggleAnimHash);
        visible = true;
    }

    public void OnLeaderboardsButtonClicked()
    {
        SocialManager.ShowLeaderboard(CoreLevelObjects.levelProperties.timeLeaderboardId);
    }

    void ResumeGame()
    {
        visible = false;
        animator.SetTrigger(toggleAnimHash);
        Time.timeScale = 1;
    }
}

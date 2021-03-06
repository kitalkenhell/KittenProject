﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    enum Views
    {
        mainScreen,
        levelSelection,
        wardrobe
    }

    public LevelProperties firstLevel;
    public PlayerBodySkin firstBodySkin;
    public PlayerHatSkin firstHatSkin;
    public PlayerParachuteSkin firstParachuteSkin;

    public CanvasGroup mainScreen;
    public CanvasGroup levelSelection;
    public CanvasGroup wardrobe;

    public float viewTransitionDuration;

    public string LevelToLoad
    {
        get;
        set;
    }

    Animator animator;
    MenuMusicManager menuMusicManager;

    static Views currentView = Views.mainScreen;
    CanvasGroup currentViewCanvas;
    bool isTransitioning;
    
    void Start() 
	{
        LevelProperties level = firstLevel;

        isTransitioning = false;

        animator = GetComponent<Animator>();
        menuMusicManager = GetComponent<MenuMusicManager>();

        PersistentData.IsHavingBodySkin(firstBodySkin.itemName, true);
        PersistentData.IsHavingHatSkin(firstHatSkin.itemName, true);
        PersistentData.IsHavingParachuteSkin(firstParachuteSkin.itemName, true);

        AdManager.Instance.IncrementEventCounter();
        SocialManager.AutoSignIn();

        PostOffice.backButtonClicked += OnBackButtonClicked;

        mainScreen.gameObject.SetActive(false);
        levelSelection.gameObject.SetActive(false);
        wardrobe.gameObject.SetActive(false);

        mainScreen.alpha = levelSelection.alpha = wardrobe.alpha = 0;

        if (currentView == Views.mainScreen)
        {
            currentViewCanvas = mainScreen;
        }
        else if (currentView == Views.wardrobe)
        {
            currentViewCanvas = wardrobe;
        }
        else
        {
            currentViewCanvas = levelSelection;
        }

        currentViewCanvas.alpha = 1;
        currentViewCanvas.gameObject.SetActive(true);

        firstLevel.IsLocked = false;

        while (level.nextLevel != null && level.nextLevel != firstLevel && level != level.nextLevel)
        {
            if (level.IsCompleted)
            {
                level.nextLevel.IsLocked = false;
            }

            level = level.nextLevel;
        }

        menuMusicManager.FadeInMusic();
    }

    void OnDestroy()
    {
        PostOffice.backButtonClicked -= OnBackButtonClicked;
    }

    IEnumerator ViewTransition(CanvasGroup nextViewCanvas, Views nextView)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;

            nextViewCanvas.gameObject.SetActive(true);
            currentViewCanvas.alpha = 1;
            nextViewCanvas.alpha = 0;

            while (currentViewCanvas.alpha > 0)
            {
                currentViewCanvas.alpha = Mathf.MoveTowards(currentViewCanvas.alpha, 0, Time.deltaTime / viewTransitionDuration);
                nextViewCanvas.alpha = 1.0f - currentViewCanvas.alpha;
                yield return null;
            }

            currentViewCanvas.gameObject.SetActive(false);
            currentViewCanvas.alpha = 0;
            nextViewCanvas.alpha = 1;

            currentView = nextView;
            currentViewCanvas = nextViewCanvas;
            isTransitioning = false; 
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            OnBackButtonClicked();
        }
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine(ViewTransition(levelSelection, Views.levelSelection));
    }

    public void OnAchievementsButtonClicked()
    {
        SocialManager.ShowAchievements();
    }

    public void OnWardrobeButtonClicked()
    {
        StartCoroutine(ViewTransition(wardrobe, Views.wardrobe));
    }

    public void OnLeaderboardsButtonClicked()
    {
        SocialManager.ShowLeaderboards();
    }

    public void OnStartGameButtonClicked()
    {
        menuMusicManager.FadeOutMusic();
        animator.enabled = true;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(LevelToLoad);
    }

    public void OnBackButtonClicked()
    {
        if (currentView == Views.mainScreen)
        {
            Application.Quit();
        }
        else if (currentView == Views.wardrobe)
        {
            StartCoroutine(ViewTransition(levelSelection, Views.levelSelection));
        }
        else
        {
            StartCoroutine(ViewTransition(mainScreen, Views.mainScreen));
        }
    }
}

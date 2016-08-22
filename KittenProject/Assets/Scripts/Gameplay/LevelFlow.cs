using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    public string mainMenuSceneName;
    public float fadeOutDelay;
    public float showHudDelay;
    public float showVictoryScreenDelay;
    public float restartDelay;
    public float adDelayAfterDeath;
    public float adDelayAfterVictory;
    public Animator loadingScreen;
    public Animator victoryScreen;
    public HudController hud;
    public PauseMenu pauseMenu;
    public LevelProperties levelProperties;

    int fadeAnimHash;
    int showVictoryScreenAnimHash;

    bool switchingScenes;
    bool showingVictoryScreen;

    void Awake()
    {
        CoreLevelObjects.levelProperties = levelProperties;
    }

    void Start()
    {
        PostOffice.playedDied += OnPlayerDied;
        PostOffice.victory += OnVictory;
        PostOffice.levelQuit += OnLevelQuit;

        fadeAnimHash = Animator.StringToHash("Fade");
        showVictoryScreenAnimHash = Animator.StringToHash("ShowVictoryScreen");

        loadingScreen.SetTrigger(fadeAnimHash);

        switchingScenes = false;
        showingVictoryScreen = false;

        StartCoroutine(ShowHud());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.gameObject.activeInHierarchy)
            {
                pauseMenu.gameObject.SetActive(true); 
            }
        }
    }

    void OnDestroy()
    {
        PostOffice.playedDied -= OnPlayerDied;
        PostOffice.victory -= OnVictory;
        PostOffice.levelQuit -= OnLevelQuit;
    }

    void OnLevelQuit()
    {
        if (!switchingScenes)
        {
            switchingScenes = true;
            hud.Hide();

            StartCoroutine(Exit());
            StartCoroutine(FadeOut());

            AnalyticsManager.OnLevelAbandoned(levelProperties.sceneName);
        }
    }

    void OnPlayerDied()
    {
        StartCoroutine(ShowAd(adDelayAfterDeath));

        hud.Hide();

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name, fadeOutDelay));
        StartCoroutine(FadeOut(fadeOutDelay));
    }

    public void OnRestartLevel()
    {
        if (!switchingScenes)
        {
            switchingScenes = true;

            if (showingVictoryScreen)
            {
                victoryScreen.SetTrigger(showVictoryScreenAnimHash);
                StartCoroutine(FadeOut(fadeOutDelay));
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name, fadeOutDelay));
            }
            else
            {
                StartCoroutine(FadeOut());
                StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
            }
        }
    }

    void OnVictory()
    {
        float time = LevelStopwatch.Stopwatch;

        if (levelProperties.nextLevel != null)
        {
            levelProperties.nextLevel.IsLocked = false;
        }

        AnalyticsManager.OnLevelCompleted(levelProperties.sceneName, time);
        SocialManager.PostLevelTimeToLeaderboard(levelProperties.timeLeaderboardId, time);

        if (CoreLevelObjects.player.GoldenKittenCollected)
        {
            levelProperties.HasGoldenKittenStar = true;
        }

        levelProperties.BestTimeScore = Mathf.Min(time, levelProperties.BestTimeScore);
        levelProperties.BestCoinsScore = Mathf.Max(CoreLevelObjects.player.Coins, levelProperties.BestCoinsScore);

        PersistentData.Coins += CoreLevelObjects.player.Coins;

        hud.Hide();
        StartCoroutine(ShowVictoryScreen());
        StartCoroutine(ShowAd(adDelayAfterVictory));
    }

    public void ShowLeaderboards()
    {
        SocialManager.ShowLeaderboard(levelProperties.timeLeaderboardId);
    }

    public void PlayNextLevel()
    {
        if (!switchingScenes && levelProperties.nextLevel != null)
        {
            switchingScenes = true;
            victoryScreen.SetTrigger(showVictoryScreenAnimHash);
            StartCoroutine(LoadLevel(levelProperties.nextLevel.sceneName, fadeOutDelay));
            StartCoroutine(FadeOut(fadeOutDelay));
        }
    }

    public void GoToMainMenu()
    {
        if (!switchingScenes)
        {
            switchingScenes = true;
            victoryScreen.SetTrigger(showVictoryScreenAnimHash);
            StartCoroutine(Exit(fadeOutDelay));
            StartCoroutine(FadeOut(fadeOutDelay)); 
        }
    }

    IEnumerator ShowAd(float delay)
    {
        yield return new WaitForSeconds(delay);
        AdManager.Instance.IncrementEventCounter();
    }

    IEnumerator ShowVictoryScreen()
    {
        yield return new WaitForSeconds(showVictoryScreenDelay);
        showingVictoryScreen = true;
        victoryScreen.SetTrigger(showVictoryScreenAnimHash);
    }

    IEnumerator ShowHud()
    {
        yield return new WaitForSeconds(showHudDelay);
        hud.Show();
    }

    IEnumerator FadeOut(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        loadingScreen.SetTrigger(fadeAnimHash);
    }

    IEnumerator LoadLevel(string sceneName, float delay = 0)
    {
        yield return new WaitForSeconds(delay + restartDelay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Exit(float delay = 0)
    {
        yield return new WaitForSeconds(delay + restartDelay);
        SceneManager.LoadScene(mainMenuSceneName);
    }
 }

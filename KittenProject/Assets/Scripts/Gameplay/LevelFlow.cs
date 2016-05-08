using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    public string mainMenuSceneName;
    public float fadeOutDelay;
    public float showHudDelay;
    public float showVictoryScreenDelay;
    public float showAdDelay;
    public float restartDelay;
    public Animator loadingScreen;
    public Animator victoryScreen;
    public Animator hud;
    public PauseMenu pauseMenu;
    public LevelProperties levelProperties;

    int fadeAnimHash;
    int showHudAnimHash;
    int showVictoryScreenAnimHash;

    bool switchingScenes;

    void Start()
    {
        PostOffice.playedDied += OnPlayerDied;
        PostOffice.victory += OnVictory;
        PostOffice.levelQuit += OnLevelQuit;

        fadeAnimHash = Animator.StringToHash("Fade");
        showHudAnimHash = Animator.StringToHash("ShowHud");
        showVictoryScreenAnimHash = Animator.StringToHash("ShowVictoryScreen");

        loadingScreen.SetTrigger(fadeAnimHash);

        switchingScenes = false;

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
            hud.SetBool(showHudAnimHash, false);

            StartCoroutine(Exit());
            StartCoroutine(FadeOut());

            AnalyticsManager.OnLevelAbandoned(levelProperties.sceneName);
        }
    }

    void OnPlayerDied()
    {
        AdManager.Instance.IncrementEventCounter();

        hud.SetBool(showHudAnimHash, false);

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
        StartCoroutine(FadeOut());
    }

    public void OnRestartLevel()
    {
        if (!switchingScenes)
        {
            switchingScenes = true;
            victoryScreen.SetTrigger(showVictoryScreenAnimHash);
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
            StartCoroutine(FadeOut()); 
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

        hud.SetBool(showHudAnimHash, false);
        StartCoroutine(ShowVictoryScreen());
        StartCoroutine(ShowAd());
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
            StartCoroutine(LoadLevel(levelProperties.nextLevel.sceneName));
            StartCoroutine(FadeOut());
        }
    }

    public void GoToMainMenu()
    {
        if (!switchingScenes)
        {
            switchingScenes = true;
            victoryScreen.SetTrigger(showVictoryScreenAnimHash);
            StartCoroutine(Exit());
            StartCoroutine(FadeOut()); 
        }
    }

    IEnumerator ShowVictoryScreen()
    {
        yield return new WaitForSeconds(showVictoryScreenDelay);
        victoryScreen.SetTrigger(showVictoryScreenAnimHash);
    }

    IEnumerator ShowAd()
    {
        yield return new WaitForSeconds(showAdDelay);
        AdManager.Instance.IncrementEventCounter();
    }

    IEnumerator ShowHud()
    {
        yield return new WaitForSeconds(showHudDelay);
        hud.SetBool(showHudAnimHash, true);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        loadingScreen.SetTrigger(fadeAnimHash);
    }

    IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(fadeOutDelay + restartDelay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(fadeOutDelay + restartDelay);
        SceneManager.LoadScene(mainMenuSceneName);
    }
 }

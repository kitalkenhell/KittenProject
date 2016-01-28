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
    public Animator loadingScreen;
    public Animator victoryScreen;
    public Animator hud;
    public LevelProperties levelProperties;


    int fadeAnimHash;
    int showHudAnimHash;
    int showVictoryScreenAnimHash;

    bool switchingScenes;

    void Start()
    {
        PostOffice.playedDied += OnPlayerDied;
        PostOffice.victory += OnVictory;

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
            if (!switchingScenes)
            {
                switchingScenes = true;
                hud.SetBool(showHudAnimHash, false);
                StartCoroutine(Exit());
                StartCoroutine(FadeOut());
            }
        }
    }

    void OnDestroy()
    {
        PostOffice.playedDied -= OnPlayerDied;
        PostOffice.victory -= OnVictory;
    }

    void OnPlayerDied()
    {
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
        if (levelProperties.nextLevel != null)
        {
            levelProperties.nextLevel.IsLocked = false;
        }

        SocialManager.PostLevelTimeToLeaderboard(levelProperties.timeLeaderboardId, LevelStopwatch.Stopwatch);
        hud.SetBool(showHudAnimHash, false);
        StartCoroutine(ShowVictoryScreen());
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

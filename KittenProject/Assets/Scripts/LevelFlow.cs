using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    public float fadeOutDelay;
    public float showHudDelay;
    public float showVictoryScreenDelay;
    public float restartDelay;
    public Animator loadingScreen;
    public Animator victoryScreen;
    public Animator hud;

    int fadeAnimHash;
    int showHudAnimHash;
    int showVictoryScreenAnimHash;

    void Start()
    {
        PostOffice.playedDied += OnPlayerDied;
        PostOffice.victory += OnVictory;
        PostOffice.restartLevel += OnRestartLevel;

        fadeAnimHash = Animator.StringToHash("Fade");
        showHudAnimHash = Animator.StringToHash("ShowHud");
        showVictoryScreenAnimHash = Animator.StringToHash("ShowVictoryScreen");

        loadingScreen.SetTrigger(fadeAnimHash);

        StartCoroutine(ShowHud());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnDestroy()
    {
        PostOffice.playedDied -= OnPlayerDied;
        PostOffice.victory -= OnVictory;
        PostOffice.restartLevel -= OnRestartLevel;
    }

    void OnPlayerDied()
    {
        hud.SetBool(showHudAnimHash, false);
        StartCoroutine(RestartLevel());
        StartCoroutine(FadeOut());
    }

    void OnRestartLevel()
    {
        //fadeOutDelay = 0;
        victoryScreen.SetTrigger(showVictoryScreenAnimHash);
        StartCoroutine(RestartLevel());
        StartCoroutine(FadeOut());
    }

    void OnVictory()
    {
        hud.SetBool(showHudAnimHash, false);
        StartCoroutine(ShowVictoryScreen());
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

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(fadeOutDelay + restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

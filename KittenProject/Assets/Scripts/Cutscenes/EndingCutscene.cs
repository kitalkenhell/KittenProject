using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EndingCutscene : MonoBehaviour
{
    const float startChattingDelay = 1.0f;
    const float theEndChattingDelay = 2.0f;
    const float showHudDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public RuntimeAnimatorController cameraRuntimeAnimatorController;
    public Animator ChihuahuaMover;
    public Credits credits;

    public CutsceneChatting endingChatting;
    public CutsceneChatting theEndChatting;

    int stepAnimHash = Animator.StringToHash("Step");

    bool canSkip = false;
    bool started = false;
    bool creditsFinished = false;

    IEnumerator StartCutscene()
    {
        canSkip = true;
        started = true;
        hud.gameObject.SetActive(false);
        movieBars.Show();
        playerController.ControlsEnabled = false;
        cutsceneCamera.GetComponent<Animator>().runtimeAnimatorController = cameraRuntimeAnimatorController;
        cutsceneCamera.gameObject.SetActive(false); //reset amimator
        cutsceneCamera.gameObject.SetActive(true);
        cutsceneCamera.enabled = true;
        LevelStopwatch.StopTimer();

        if (PersistentData.HasWatchedBossFightCutscene)
        {
            skipButton.Show();
        }

        yield return new WaitForSeconds(startChattingDelay);

        endingChatting.Chat();
        yield return new WaitUntil(() => { return endingChatting.HasEnded; });

        ChihuahuaMover.SetTrigger(stepAnimHash);

        yield return new WaitForSeconds(theEndChattingDelay);

        theEndChatting.Chat();
        yield return new WaitUntil(() => { return theEndChatting.HasEnded; });

        credits.gameObject.SetActive(true);
        credits.onFinished += OnCreditsFinished;

        if (!PersistentData.HasWatchedBossFightCutscene)
        {
            PersistentData.HasWatchedBossFightCutscene = true;
            skipButton.Show();
        }

        yield return new WaitUntil(() => { return creditsFinished; });

        canSkip = false;
        skipButton.Hide();
        cutsceneCamera.NextShot();
        hud.gameObject.SetActive(true);
        movieBars.Hide();
        credits.gameObject.SetActive(false);
        credits.onFinished -= OnCreditsFinished;

        yield return new WaitForSeconds(showHudDelay);

        cutsceneCamera.enabled = false;
        PostOffice.PostVictory();
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        canSkip = false;
        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();
        cutsceneCamera.Skip();
        skipButton.Hide();
        endingChatting.Skip();
        credits.gameObject.SetActive(false);
        credits.onFinished -= OnCreditsFinished;

        yield return new WaitForSeconds(fadedDuration);
        moveFade.Hide();

        cutsceneCamera.enabled = false;
        PostOffice.PostVictory();

        yield return new WaitForSeconds(showHudDelay);
        hud.gameObject.SetActive(true);
    }

    void OnCreditsFinished()
    {
        creditsFinished = true;
    }

    public void Skip()
    {
        if (canSkip)
        {
            StopAllCoroutines();
            StartCoroutine(SkipAnimation());
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!started)
        {
            StartCoroutine(StartCutscene());
        }
    }
}

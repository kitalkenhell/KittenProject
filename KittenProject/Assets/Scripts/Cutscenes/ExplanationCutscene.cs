using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ExplanationCutscene : MonoBehaviour
{
    const float startChattingDelay = 2.3f;
    const float endDelay = 2.0f;
    const float showHudDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public Animator lift;

    public CutsceneChatting chatting;

    int upAnimHash = Animator.StringToHash("Up");

    DogCutsceneController dogCutsceneController;
    bool canSkip = false;

    IEnumerator Start()
    {
        dogCutsceneController = playerController.GetComponent<DogCutsceneController>();
        canSkip = true;
        hud.gameObject.SetActive(false);
        movieBars.Show();
        playerController.enabled = false;
        dogCutsceneController.enabled = true;

        if (PersistentData.HasWatchedExplanationCutscene)
        {
            skipButton.Show(); 
        }

        yield return new WaitForSeconds(startChattingDelay);

        chatting.Chat();
        yield return new WaitUntil(() => { return chatting.HasEnded; });

        lift.SetTrigger(upAnimHash);

        yield return new WaitForSeconds(endDelay);

        canSkip = false;
        skipButton.Hide();
        cutsceneCamera.NextShot();
        hud.gameObject.SetActive(true);
        lift.gameObject.SetActive(false);
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;

        yield return new WaitForSeconds(showHudDelay);
        hud.Show();
        cutsceneCamera.enabled = false;

        PersistentData.HasWatchedExplanationCutscene = true;
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();
        lift.gameObject.SetActive(false);

        cutsceneCamera.Skip();
        skipButton.Hide();
        chatting.Skip();

        yield return new WaitForSeconds(fadedDuration);
        moveFade.Hide();

        cutsceneCamera.enabled = false;
        playerController.enabled = true;

        yield return new WaitForSeconds(showHudDelay);
        hud.gameObject.SetActive(true);
        hud.Show();
    }

    public void Skip()
    {
        if (canSkip)
        {
            StopAllCoroutines();
            StartCoroutine(SkipAnimation()); 
        }
    }
}
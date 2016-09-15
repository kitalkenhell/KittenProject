using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DragonCutscene : MonoBehaviour
{
    const float startChattingDelay = 1.0f;
    const float fadeoutDuration = 1.5f;
    const float showHudDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public Hellfire hellfire;

    public CutsceneChatting chatting;

    DogCutsceneController dogCutsceneController;
    bool canSkip = false;

    IEnumerator Start()
    {
        dogCutsceneController = playerController.GetComponent<DogCutsceneController>();
        canSkip = true;
        hud.gameObject.SetActive(false);
        movieBars.Show();
        hellfire.gameObject.SetActive(false);
        playerController.enabled = false;
        dogCutsceneController.enabled = true;

        if (PersistentData.HasWatchedDragonCutscene)
        {
            skipButton.Show(); 
        }

        yield return new WaitForSeconds(startChattingDelay);

        chatting.Chat();
        yield return new WaitUntil(() => { return chatting.HasEnded; });

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);
        playerController.gameObject.SetActive(true);
        cutsceneCamera.Skip();
        moveFade.Hide();

        canSkip = false;
        skipButton.Hide();
        hud.gameObject.SetActive(true);
        hellfire.gameObject.SetActive(true);
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;

        yield return new WaitForSeconds(showHudDelay);
        hud.Show();
        cutsceneCamera.enabled = false;

        PersistentData.HasWatchedDragonCutscene = true;
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();

        cutsceneCamera.Skip();
        skipButton.Hide();

        hellfire.gameObject.SetActive(true);
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
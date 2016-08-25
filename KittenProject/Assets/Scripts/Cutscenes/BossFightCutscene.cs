using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BossFightCutscene : MonoBehaviour
{
    const float startChattingDelay = 3.0f;
    const float endDelay = 2.0f;
    const float showHudDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public CutsceneCharacterController chihuahua;
    public GameObject dragon;
    public GameObject flameThrowers;

    public CutsceneChatting bossFightChatting;

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
        chihuahua.MoveToNextPoint();

        if (PersistentData.HasWatchedBossFightCutscene)
        {
            skipButton.Show(); 
        }

        yield return new WaitForSeconds(startChattingDelay);

        bossFightChatting.Chat();
        yield return new WaitUntil(() => { return bossFightChatting.HasEnded; });

        chihuahua.transform.FlipX();
        chihuahua.MoveToNextPoint();

        yield return new WaitForSeconds(endDelay);

        canSkip = false;
        skipButton.Hide();
        cutsceneCamera.NextShot();
        hud.gameObject.SetActive(true);
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;
        chihuahua.gameObject.SetActive(false);
        dragon.SetActive(true);
        flameThrowers.SetActive(false);

        yield return new WaitForSeconds(showHudDelay);
        hud.Show();

        PersistentData.HasWatchedBossFightCutscene = true;
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();
        chihuahua.gameObject.SetActive(false);
        dragon.SetActive(true);
        flameThrowers.SetActive(false);

        cutsceneCamera.Skip();
        skipButton.Hide();

        bossFightChatting.Skip();

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
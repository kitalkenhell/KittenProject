using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class IntroCutscene : MonoBehaviour
{
    const float startChattingDelay = 1.0f;
    const float fadeoutDuration = 1.5f;
    const float delayAfterFadeout = 1.5f;
    const float distanceThresholdToContinue = 10.0f;
    const float sleepAnimationDuration = 4.0f;
    const float showHudDelay = 1.0f;
    const float startTutorialDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public CutsceneCharacterController king;
    public CutsceneCharacterController lord;
    public CutsceneCharacterController chihuahua;
    public GameObject sleepingDoge;
    public Transform dogePivot;
    public GameObject kitten;
    public Animator tutorial;

    public CutsceneChatting welcomeChatting;
    public CutsceneChatting introduceChihuahuaChatting;
    public CutsceneChatting afterIntroductionChatting;
    public CutsceneChatting dogeWakesUpChatting;
    public CutsceneChatting endingChatting;

    int stepAnimHash = Animator.StringToHash("Step");

    DogCutsceneController dogCutsceneController;
    bool canSkip = false;

    IEnumerator Start()
    {
        dogCutsceneController = playerController.GetComponent<DogCutsceneController>();
        canSkip = true;
        sleepingDoge.SetActive(false);
        hud.gameObject.SetActive(false);
        movieBars.Show();
        playerController.enabled = false;
        dogCutsceneController.enabled = true;

        skipButton.Show();
        king.MoveToNextPoint();
        yield return new WaitUntil(() => { return king.HasReachedCurrentPoint; });

        yield return new WaitForSeconds(startChattingDelay);

        welcomeChatting.Chat();
        yield return new WaitUntil(() => { return welcomeChatting.HasEnded; });

        lord.MoveToNextPoint();
        yield return new WaitUntil(() => { return lord.DistanceToCurrentPoint < distanceThresholdToContinue; });

        introduceChihuahuaChatting.Chat();
        yield return new WaitUntil(() => { return introduceChihuahuaChatting.HasEnded; });

        chihuahua.MoveToNextPoint();
        yield return new WaitUntil(() => { return chihuahua.DistanceToCurrentPoint < distanceThresholdToContinue; });

        afterIntroductionChatting.Chat();
        yield return new WaitUntil(() => { return afterIntroductionChatting.HasEnded; });

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        king.gameObject.SetActive(false);
        lord.gameObject.SetActive(false);
        chihuahua.gameObject.SetActive(false);
        cutsceneCamera.NextShot();
        sleepingDoge.SetActive(true);
        playerController.gameObject.SetActive(false);
        moveFade.Hide();

        yield return new WaitForSeconds(sleepAnimationDuration);

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);
        sleepingDoge.SetActive(false);
        playerController.gameObject.SetActive(true);
        cutsceneCamera.NextShot();
        kitten.SetActive(false);
        moveFade.Hide();

        yield return new WaitForSeconds(delayAfterFadeout);

        dogeWakesUpChatting.Chat();
        yield return new WaitUntil(() => { return dogeWakesUpChatting.HasEnded; });

        dogePivot.FlipX();

        endingChatting.Chat();
        yield return new WaitUntil(() => { return endingChatting.HasEnded; });

        canSkip = false;
        skipButton.Hide();
        dogePivot.FlipX();
        cutsceneCamera.NextShot();
        hud.gameObject.SetActive(true);
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;

        yield return new WaitForSeconds(showHudDelay);
        hud.Show();

        yield return new WaitForSeconds(startTutorialDelay);

        tutorial.SetTrigger(stepAnimHash);
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();
        king.gameObject.SetActive(false);
        lord.gameObject.SetActive(false);
        chihuahua.gameObject.SetActive(false);
        sleepingDoge.gameObject.SetActive(false);
        kitten.gameObject.SetActive(false);

        cutsceneCamera.Skip();
        skipButton.Hide();

        welcomeChatting.Skip();
        introduceChihuahuaChatting.Skip();
        afterIntroductionChatting.Skip();
        dogeWakesUpChatting.Skip();
        endingChatting.Skip();
        
        yield return new WaitForSeconds(fadedDuration);
        moveFade.Hide();

        cutsceneCamera.enabled = false;
        playerController.enabled = true;

        yield return new WaitForSeconds(showHudDelay);
        hud.gameObject.SetActive(true);
        hud.Show();

        yield return new WaitForSeconds(startTutorialDelay);

        tutorial.SetTrigger(stepAnimHash);
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
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class IntroCutscene : MonoBehaviour
{
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public CutsceneCharacterController king;
    public CutsceneCharacterController lord;
    public CutsceneCharacterController chihuahua;

    public CutsceneChatting welcomeChatting;
    public CutsceneChatting IntroduceChihuahuaChatting;
    public CutsceneChatting AfterIntroductionChatting;
    public CutsceneChatting EndingChatting;

    DogCutsceneController dogCutsceneController;

    IEnumerator Start()
    {
        const float startChattingDelay = 1.0f;
        const float fadeoutDuration = 1.5f;
        const float delayAfterFadeout = 1.5f;
        const float distanceThresholdToContinue = 10.0f;

        dogCutsceneController = playerController.GetComponent<DogCutsceneController>();

        hud.gameObject.SetActive(false);
        movieBars.Show();
        playerController.enabled = false;
        dogCutsceneController.enabled = true;

        king.MoveToNextPoint();
        yield return new WaitUntil(() => { return king.HasReachedCurrentPoint; });

        yield return new WaitForSeconds(startChattingDelay);

        welcomeChatting.Chat();
        yield return new WaitUntil(() => { return welcomeChatting.HasEnded; });

        lord.MoveToNextPoint();
        yield return new WaitUntil(() => { return lord.DistanceToCurrentPoint < distanceThresholdToContinue; });

        IntroduceChihuahuaChatting.Chat();
        yield return new WaitUntil(() => { return IntroduceChihuahuaChatting.HasEnded; });

        chihuahua.MoveToNextPoint();
        yield return new WaitUntil(() => { return chihuahua.DistanceToCurrentPoint < distanceThresholdToContinue; });

        AfterIntroductionChatting.Chat();
        yield return new WaitUntil(() => { return AfterIntroductionChatting.HasEnded; });

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        king.gameObject.SetActive(false);
        lord.gameObject.SetActive(false);
        chihuahua.gameObject.SetActive(false);
        cutsceneCamera.NextShot();
        moveFade.Hide();

        yield return new WaitForSeconds(delayAfterFadeout);

        EndingChatting.Chat();
        yield return new WaitUntil(() => { return EndingChatting.HasEnded; });

        hud.gameObject.SetActive(true);
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;
    }
}

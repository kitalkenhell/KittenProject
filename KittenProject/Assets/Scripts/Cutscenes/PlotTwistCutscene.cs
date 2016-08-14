using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlotTwistCutscene : MonoBehaviour
{
    const float startChattingDelay = 1.5f;
    const float guardIntroChattingDelay = 1.2f;
    const float outroChattingDelay = 1.2f;
    const float showHudDelay = 1.0f;

    const float riseHellfireDelay = 2.0f;
    const float hellfireRisingSpeed = 2;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public CutsceneCharacterController knight;
    public Animator lift;
    public GameObject fireballThrowers;
    public GameObject flames;
    public Hellfire hellfire;
    public Transform hellfirePositionAfterCutscene;
    public float hellfireSpeed;

    public CutsceneChatting introChatting;
    public CutsceneChatting guardIntroChatting;
    public CutsceneChatting outroChatting;

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

        if (PersistentData.HasWatchedPlotTwistCutscene)
        {
            skipButton.Show(); 
        }

        yield return new WaitForSeconds(startChattingDelay);

        introChatting.Chat();
        yield return new WaitUntil(() => { return introChatting.HasEnded; });

        knight.MoveToNextPoint();

        yield return new WaitForSeconds(guardIntroChattingDelay);

        guardIntroChatting.Chat();
        yield return new WaitUntil(() => { return guardIntroChatting.HasEnded; });

        lift.SetTrigger(upAnimHash);

        yield return new WaitForSeconds(outroChattingDelay);

        StartCoroutine(MoveHellfire());
        fireballThrowers.SetActive(true);

        outroChatting.Chat();
        yield return new WaitUntil(() => { return outroChatting.HasEnded; });

        canSkip = false;
        skipButton.Hide();
        cutsceneCamera.NextShot();
        hud.gameObject.SetActive(true);
        lift.gameObject.SetActive(false);
        fireballThrowers.SetActive(false);
        knight.GetComponent<Knight>().enabled = true;
        knight.enabled = false;
        movieBars.Hide();
        playerController.enabled = true;
        dogCutsceneController.enabled = false;
        hellfire.speed = hellfireSpeed;

        yield return new WaitForSeconds(showHudDelay);
        hud.Show();

        PersistentData.HasWatchedPlotTwistCutscene = true;
    }

    IEnumerator SkipAnimation()
    {
        const float fadeoutDuration = 1.0f;
        const float fadedDuration = 0.6f;

        moveFade.Show();
        yield return new WaitForSeconds(fadeoutDuration);

        movieBars.Hide();
        lift.gameObject.SetActive(false);
        knight.GetComponent<Knight>().enabled = true;
        knight.transform.position = knight.waypoints.Last().position;
        knight.enabled = false;
        playerController.gameObject.SetActive(true);
        fireballThrowers.SetActive(false);
        flames.SetActive(true);
        hellfire.transform.SetPositionY(hellfirePositionAfterCutscene.position.y);
        hellfire.speed = hellfireSpeed;

        cutsceneCamera.Skip();
        skipButton.Hide();

        introChatting.Skip();
        guardIntroChatting.Skip();
        outroChatting.Skip();

        yield return new WaitForSeconds(fadedDuration);
        moveFade.Hide();

        cutsceneCamera.enabled = false;
        playerController.enabled = true;

        yield return new WaitForSeconds(showHudDelay);
        hud.gameObject.SetActive(true);
        hud.Show();
    }

    IEnumerator MoveHellfire()
    {
        yield return new WaitForSeconds(riseHellfireDelay);

        flames.SetActive(true);

        while (hellfire.transform.position.y + Mathf.Epsilon < hellfirePositionAfterCutscene.position.y)
        {
            hellfire.transform.SetPositionY(Mathf.MoveTowards(hellfire.transform.position.y, hellfirePositionAfterCutscene.position.y, Time.deltaTime * hellfireRisingSpeed));
            yield return null;
        }

        fireballThrowers.SetActive(false);
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
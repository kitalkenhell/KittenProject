using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class FreefallCutscene : MonoBehaviour
{
    const float startChattingDelay = 1.5f;
    const float theEndChattingDelay = 2.0f;
    const float liftDelay = 2.5f;
    const float enableControlsDelay = 1.0f;
    const float showHudDelay = 1.0f;

    public SkipCutsceneButton skipButton;
    public HudController hud;
    public MovieBarsController movieBars;
    public MovieFadeController moveFade;
    public PlayerController playerController;
    public CutsceneCamera cutsceneCamera;
    public GameObject platform;
    public Rigidbody2D[] destroyedPlatformChunks;
    public Rigidbody2D morningStar;
    public Animator lift;
    public Transform dogePositionAfterCutscene;

    public CutsceneChatting firstChatting;
    public CutsceneChatting secondChatting;
    
    bool canSkip = false;
    bool started = false;

    IEnumerator StartCutscene()
    {
        canSkip = true;
        started = true;
        hud.Hide();
        movieBars.Show();
        playerController.ControlsEnabled = false;
        cutsceneCamera.enabled = true;

        if (PersistentData.HasWatchedFreefallCutscene)
        {
            skipButton.Show();
        }

        yield return new WaitForSeconds(startChattingDelay);

        firstChatting.Chat();
        yield return new WaitUntil(() => { return firstChatting.HasEnded; });

        lift.enabled = true;
        yield return new WaitForSeconds(liftDelay);

        secondChatting.Chat();
        yield return new WaitUntil(() => { return secondChatting.HasEnded; });

        canSkip = false;
        StartCoroutine(DestroyPlatform());

        yield return new WaitForSeconds(enableControlsDelay);

        if (!PersistentData.HasWatchedFreefallCutscene)
        {
            PersistentData.HasWatchedFreefallCutscene = true;
            skipButton.Show();
        }

        skipButton.Hide();
        cutsceneCamera.NextShot();
        hud.Show();
        movieBars.Hide();

        yield return new WaitForSeconds(showHudDelay);

        playerController.ControlsEnabled = true;
        cutsceneCamera.enabled = false;
    }

    public IEnumerator DestroyPlatform()
    {
        const float angularVelocityMin = 80;
        const float angularVelocityMax = 90;
        const float velocityMin = 20;
        const float velocityMax = 25;
        const float destroyDelay = 3.0f;
        const float delay = 0.95f;

        morningStar.isKinematic = false;

        yield return new WaitForSeconds(delay);

        Destroy(platform);

        for (int i = 0; i < destroyedPlatformChunks.Length; i++)
        {
            destroyedPlatformChunks[i].gameObject.SetActive(true);
            destroyedPlatformChunks[i].angularVelocity = Random.Range(angularVelocityMin, angularVelocityMax);
            destroyedPlatformChunks[i].velocity = Vector3.down * Random.Range(velocityMin, velocityMax);
            Destroy(destroyedPlatformChunks[i].gameObject, destroyDelay);
        }

        Destroy(morningStar.transform.parent.gameObject, destroyDelay);
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
        firstChatting.Skip();
        secondChatting.Skip();
        CoreLevelObjects.player.transform.SetPositionXY(dogePositionAfterCutscene.position.XY());

        yield return new WaitForSeconds(fadedDuration);
        moveFade.Hide();

        cutsceneCamera.enabled = false;

        yield return new WaitForSeconds(showHudDelay);
        hud.gameObject.SetActive(true);
        hud.Show();

        playerController.ControlsEnabled = true;
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

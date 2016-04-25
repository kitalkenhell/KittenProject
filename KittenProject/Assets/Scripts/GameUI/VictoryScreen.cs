using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryScreen : MonoBehaviour
{
    const float revealNextAwardDelay = 0.5f;
    const float showButtonsDelay = 0.25f;

    public LevelFlow levelFlow;
    public Animator hud;
    public Image kittenGreyedOut;
    public Image kitten;
    public GameObject kittenChunks;
    public Image gemGreyedOut;
    public Image gem;
    public GameObject gemChunks;
    public Image hourglassGreyedOut;
    public Image hourglass;
    public GameObject hourglassChunks;

    Animator animator;

    int showButtonsAnimHash;
    int showCoinsAndTimerAnimHash;

    bool kittenFound;
    bool gemsCollected;
    bool timeTrialCompleted;

    void Start()
    {
        animator = GetComponent<Animator>();

        showButtonsAnimHash = Animator.StringToHash("ShowButtons");
        showCoinsAndTimerAnimHash = Animator.StringToHash("ShowCoinsAndTimer");

        kittenFound = levelFlow.levelProperties.HasGoldenKittenStar;
        gemsCollected = levelFlow.levelProperties.HasCoinStar;
        timeTrialCompleted = levelFlow.levelProperties.HasTimeStar;

        kittenGreyedOut.enabled = !kittenFound;
        kitten.enabled = kittenFound;

        gemGreyedOut.enabled = !gemsCollected;
        gem.enabled = gemsCollected;

        hourglassGreyedOut.enabled = !timeTrialCompleted;
        hourglass.enabled = timeTrialCompleted;
    }

    public void ShowCoinsAndTimer()
    {
        hud.SetTrigger(showCoinsAndTimerAnimHash);
    }

    public IEnumerator RevealAwards()
    {
        bool wait = false;

        if (!gemsCollected && levelFlow.levelProperties.HasCoinStar)
        {
            gem.enabled = true;
            gemGreyedOut.enabled = false;
            gemChunks.SetActive(true);
            wait = true;
        }

        if (!kittenFound && levelFlow.levelProperties.HasGoldenKittenStar)
        {
            if (wait)
            {
                yield return new WaitForSeconds(revealNextAwardDelay);
            }

            kitten.enabled = true;
            kittenGreyedOut.enabled = false;
            kittenChunks.SetActive(true);
            wait = true;
        }

        if (!timeTrialCompleted && levelFlow.levelProperties.HasTimeStar)
        {
            if (wait)
            {
                yield return new WaitForSeconds(revealNextAwardDelay);
            }

            hourglass.enabled = true;
            hourglassGreyedOut.enabled = false;
            hourglassChunks.SetActive(true);
        }

        yield return new WaitForSeconds(showButtonsDelay);

        animator.SetTrigger(showButtonsAnimHash);
    }
}

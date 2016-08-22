using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class VictoryScreen : MonoBehaviour
{
    const float revealNextAwardDelay = 0.5f;
    const float showButtonsDelay = 0.25f;

    public LevelFlow levelFlow;
    public Animator hud;
    public Animator adButton;
    public RaffleButton raffleButton;
    public Image kittenGreyedOut;
    public Image kitten;
    public Image gemGreyedOut;
    public Image gem;
    public Image hourglassGreyedOut;
    public Image hourglass;
    public Text getGemsLabel;
    public Text finishLevelLabel;

    Animator animator;
    Animator kittenAnimator;
    Animator gemAnimator;
    Animator hourglassAnimator;

    int showButtonsAnimHash;
    int showCoinsAndTimerAnimHash;
    int hideAnimHash;

    bool kittenFound;
    bool gemsCollected;
    bool timeTrialCompleted;

    void Start()
    {
        animator = GetComponent<Animator>();

        showButtonsAnimHash = Animator.StringToHash("ShowButtons");
        showCoinsAndTimerAnimHash = Animator.StringToHash("ShowCoinsAndTimer");
        hideAnimHash = Animator.StringToHash("Hide");

        kittenFound = levelFlow.levelProperties.HasGoldenKittenStar;
        gemsCollected = levelFlow.levelProperties.HasCoinStar;
        timeTrialCompleted = levelFlow.levelProperties.HasTimeStar;

        kittenGreyedOut.enabled = !kittenFound;
        kitten.enabled = kittenFound;
        kittenAnimator = kittenGreyedOut.GetComponent<Animator>();

        gemGreyedOut.enabled = !gemsCollected;
        gem.enabled = gemsCollected;
        gemAnimator = gemGreyedOut.GetComponent<Animator>();

        hourglassGreyedOut.enabled = !timeTrialCompleted;
        hourglass.enabled = timeTrialCompleted;
        hourglassAnimator = hourglassGreyedOut.GetComponent<Animator>();

        getGemsLabel.text = String.Format(Strings.VictoryScreen.getGems, levelFlow.levelProperties.coinsToGetStar);
        finishLevelLabel.text = String.Format(Strings.VictoryScreen.finishLevel, levelFlow.levelProperties.timeToGetStar);

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
            gemAnimator.enabled = true;
            wait = true;
        }

        if (!kittenFound && levelFlow.levelProperties.HasGoldenKittenStar)
        {
            if (wait)
            {
                yield return new WaitForSeconds(revealNextAwardDelay);
            }

            kittenAnimator.enabled = true;
            wait = true;
        }

        if (!timeTrialCompleted && levelFlow.levelProperties.HasTimeStar)
        {
            if (wait)
            {
                yield return new WaitForSeconds(revealNextAwardDelay);
            }

            hourglassAnimator.enabled = true;
        }

        if (AdManager.Instance.IsVideoAdAvailable)
        {
            yield return new WaitForSeconds(showButtonsDelay);
            adButton.gameObject.SetActive(true);
        }

        if (raffleButton.CanEnter())
        {
            yield return new WaitForSeconds(showButtonsDelay);
            raffleButton.gameObject.SetActive(true); 
        }

        yield return new WaitForSeconds(showButtonsDelay);
        animator.SetTrigger(showButtonsAnimHash);
    }

    public void OnPlayVideoAd()
    {
        AdManager.Instance.ShowVideoAd(CoreLevelObjects.player.Coins * AdManager.coinsMultiplier);
        adButton.SetTrigger(hideAnimHash);
    }
}

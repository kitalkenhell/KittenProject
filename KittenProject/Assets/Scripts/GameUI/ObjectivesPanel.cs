using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesPanel : MonoBehaviour
{
    public Text timeLabel;
    public Text gemsLabel;

    public GameObject kitten;
    public GameObject kittenGreyedOut;
    public GameObject hourglass;
    public GameObject hourglassGreyedOut;
    public GameObject gem;
    public GameObject gemGreyedOut;
    public HudController hud;

    public float showDelay;
    public float autoHideDelay;

    int showAnimHash = Animator.StringToHash("Show");

    Animator animator;

    bool autoHide = false;
    bool wasAlreadyVisible = false;

    void Start()
    {
        LevelProperties level = CoreLevelObjects.levelProperties;

        animator = GetComponent<Animator>();

        PostOffice.levelStopwatchStarted += OnLevelStarted;

        gem.SetActive(level.HasCoinStar);
        gemGreyedOut.SetActive(!level.HasCoinStar);

        kitten.SetActive(level.HasGoldenKittenStar);
        kittenGreyedOut.SetActive(!level.HasGoldenKittenStar);

        hourglass.SetActive(level.HasTimeStar);
        hourglassGreyedOut.SetActive(!level.HasTimeStar);

        timeLabel.text = Mathf.RoundToInt(level.timeToGetStar).ToString() + "\"";
        gemsLabel.text = level.coinsToGetStar.ToString();
    }

    void OnEnable()
    {
        StartCoroutine(Show());
    }

    void OnDestroy()
    {
        PostOffice.levelStopwatchStarted -= OnLevelStarted;
    }

    IEnumerator Show()
    {
        if (!wasAlreadyVisible)
        {
            yield return new WaitForSeconds(showDelay);

            LevelProperties level = CoreLevelObjects.levelProperties;

            if (level.IsCompleted && (!level.HasCoinStar || !level.HasGoldenKittenStar || !level.HasTimeStar))
            {
                if (!hud.IsVisible)
                {
                    yield return new WaitUntil(() => hud.IsVisible);
                    yield return new WaitForSeconds(showDelay);
                }

                wasAlreadyVisible = true;
                animator.SetBool(showAnimHash, true);

                if (autoHide)
                {
                    yield return new WaitForSeconds(autoHideDelay);

                    animator.SetBool(showAnimHash, false);
                }
            } 
        }
    }

    void OnLevelStarted()
    {
        PostOffice.levelStopwatchStarted -= OnLevelStarted;
        autoHide = true;
        animator.SetBool(showAnimHash, false);
    }
}

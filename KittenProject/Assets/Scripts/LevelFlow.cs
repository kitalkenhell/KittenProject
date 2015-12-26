﻿using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement; //unity 5.3

public class LevelFlow : MonoBehaviour
{
    public float fadeOutDelay;
    public float restartDelay;
    public Animator loadingScreen;

    int fadeAnimHash;

	void Start ()
    {
        PostOffice.playedDied += OnPlayerDied;
        fadeAnimHash = Animator.StringToHash("Fade");
        loadingScreen.SetTrigger(fadeAnimHash);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnDestroy()
    {
        PostOffice.playedDied -= OnPlayerDied;
    }

    void OnPlayerDied()
    {
        StartCoroutine(RestartLevel());
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        loadingScreen.SetTrigger(fadeAnimHash);
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(restartDelay);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //unity 5.3
        Application.LoadLevel(Application.loadedLevel);
    }
}
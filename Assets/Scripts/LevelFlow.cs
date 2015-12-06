using UnityEngine;
using System.Collections;

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
        Application.LoadLevel(Application.loadedLevel);
    }
}

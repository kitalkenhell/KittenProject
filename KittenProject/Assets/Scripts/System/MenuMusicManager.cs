using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{
    const float fadeDuration = 0.5f;
    const float fullVolume = 1;
    const float mutedVolume = 0;

    public AudioClip[] music;

    AudioSource musicSource;

    void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float timer = 0;

        do
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(mutedVolume, fullVolume, timer / fadeDuration);

            yield return null;
        }
        while (timer < fadeDuration);

        musicSource.enabled = true;
    }

    IEnumerator FadeOut()
    {
        float timer = 0;

        do
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(fullVolume, mutedVolume, timer / fadeDuration);

            yield return null;
        }
        while (timer < fadeDuration);

        musicSource.enabled = false;
    }
}

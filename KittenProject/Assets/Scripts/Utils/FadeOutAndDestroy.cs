using UnityEngine;
using System.Collections;

public class FadeOutAndDestroy : MonoBehaviour
{
    const float threshold = 0.01f; 

    public float fadeOutSpeed;
    public float trialFadeOutSpeed;
    public float delay;
    public SpriteRenderer[] sprites;
    public TrailRenderer[] trailRenderers;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float maxAlpha;

        yield return new WaitForSeconds(delay);

        do
        {
            maxAlpha = 0;

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].SetAlpha(Mathf.MoveTowards(sprites[i].color.a, 0, Time.deltaTime * fadeOutSpeed));
                maxAlpha = Mathf.Max(maxAlpha, sprites[i].color.a);
            }

            for (int i = 0; i < trailRenderers.Length; i++)
            {
                trailRenderers[i].time = Mathf.MoveTowards(trailRenderers[i].time, 0, Time.deltaTime * trialFadeOutSpeed);

                sprites[i].SetAlpha(Mathf.MoveTowards(sprites[i].color.a, 0, Time.deltaTime * fadeOutSpeed));
            }

            yield return null;
        }
        while (maxAlpha > threshold);

        Destroy(gameObject);
    }
}

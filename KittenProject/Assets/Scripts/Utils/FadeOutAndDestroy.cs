using UnityEngine;
using System.Collections;

public class FadeOutAndDestroy : MonoBehaviour
{
    const float threshold = 0.01f; 

    public float fadeOutSpeed;
    public float delay;
    public SpriteRenderer[] sprites;

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

            yield return null;
        }
        while (maxAlpha > threshold);

        Destroy(gameObject);
    }
}

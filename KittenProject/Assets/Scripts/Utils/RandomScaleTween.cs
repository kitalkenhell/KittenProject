using UnityEngine;
using System.Collections;

public class RandomScaleTween : MonoBehaviour
{
    public AnimationCurve curve;
    public MinMax speedRange;
    public MinMax scale;
    public bool randomOffset;

    float time;

    void Start()
    {
        time = randomOffset ? Random.value : 0;
    }

    void Update()
    {
        time += Time.deltaTime * speedRange.Random();

        if (time > 1)
        {
            time -= 1;
        }

        transform.localScale = Vector3.one * (scale.min + (scale.max - scale.min) * curve.Evaluate(time));

    }
}

using UnityEngine;
using System.Collections;

public class FlameRandomizer : MonoBehaviour 
{
    public string stateName;
    public float AnimOffsetMin = 0.0f;
    public float AnimOffsetMax = 1.0f;

    public float scaleMin = 0.7f;
    public float scaleMax = 1.0f;

    public float rotationMin = -10.0f;
    public float rotationMax = 10.0f;

    public Vector2 offsetMin = new Vector2(-1, -1);
    public Vector2 offsetMax = new Vector2(1, 1);

    void Start()
    {
        const float sortingOrderMin = 0;
        const float sortingOrderMax = 10.0f;

        float scale = Random.Range(scaleMin, scaleMax);
        Vector3 offset = Vector3.zero;

        offset.x = Random.Range(offsetMin.x, offsetMax.x);
        offset.y = Random.Range(offsetMin.y, offsetMax.y);

        GetComponent<Animator>().Play(stateName, 0, Random.Range(AnimOffsetMin, AnimOffsetMax));
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(Random.Range(sortingOrderMin, sortingOrderMax));

        transform.localScale = new Vector3(scale, scale, scale);
        transform.eulerAngles = new Vector3(0, 0, Random.Range(rotationMin, rotationMax));
        transform.position += offset;
    }
}

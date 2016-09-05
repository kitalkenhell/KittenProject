using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEngine.UI;

public class Utils
{
    public static float RandomSign()
    {
        return UnityEngine.Random.value < 0.5f ? -1.0f : 1.0f;
    }

    public static Vector2 PerpendicularVector2(Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }

    public static void ReplaceSpritesWithUiImages(GameObject gameobject, bool addCanvas)
    {
        SpriteRenderer[] renderers;

        renderers = gameobject.GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            int depth = renderer.sortingOrder;

            Image image = renderer.gameObject.AddComponent<Image>();
            image.sprite = renderer.sprite;

            if (addCanvas)
            {
                const int sortingOrderOffset = 100;

                Canvas canvas = image.gameObject.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = depth + sortingOrderOffset;
            }

            GameObject.Destroy(renderer);
        }
    }

    public static void DestroyChildren(Transform t)
    {
        foreach (Transform child in t)
        {
            GameObject.Destroy(child);
        }
    }
}


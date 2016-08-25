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

    public static void ReplaceSpritesWithUiImages(GameObject gameobject)
    {
        SpriteRenderer[] renderers;

        renderers = gameobject.GetComponentsInChildren<SpriteRenderer>();

        foreach (var renderer in renderers)
        {
            Image image = renderer.gameObject.AddComponent<Image>();
            image.sprite = renderer.sprite;
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


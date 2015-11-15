using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

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
}


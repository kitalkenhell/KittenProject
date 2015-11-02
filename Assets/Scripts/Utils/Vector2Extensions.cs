﻿using UnityEngine;
using System.Collections;

public static class Vector2Extensions
{
    public static Vector3 Vec3(this Vector2 v)
    {
        return new Vector3(v.x, v.x, 0);
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class SpriteRendererExtensions
{
    public static void SetAlpha(this SpriteRenderer sprite, float alpha)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }
}
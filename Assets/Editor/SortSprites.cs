using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;
using System.Reflection;

public class SortSprites : MonoBehaviour 
{

    [MenuItem("Ulility/Sort Sprites")]
    static void Sort()
    {
        const float offset = 0.02f;

        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();
        string[] layers = GetSortingLayerNames();

        foreach (var sprite in sprites)
        {
            sprite.transform.SetPositionZ(-offset * Array.IndexOf(layers, sprite.sortingLayerName));
        }
    }

    static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}

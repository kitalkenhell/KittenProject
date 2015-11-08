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
        const float offset = 0.03f;

        Renderer[] sprites = FindObjectsOfType<Renderer>();
        string[] layers = EditorUtils.GetSortingLayerNames();

        foreach (var sprite in sprites)
        {
            sprite.transform.SetPositionZ(-offset * Array.IndexOf(layers, sprite.sortingLayerName));
        }
    }
}

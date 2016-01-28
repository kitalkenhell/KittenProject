using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;
using System.Reflection;
using Spriter2UnityDX;

public class SortObjects : MonoBehaviour 
{

    [MenuItem("Ulility/Sort Objects")]
    static void Sort()
    {
        const float offset = 1.0f;
        const float baseOffset = 20.0f;

        Renderer[] sprites = FindObjectsOfType<Renderer>();
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        Camera camera = FindObjectOfType<Camera>();
        string[] layers = EditorUtils.GetSortingLayerNames();
        EntityRenderer[] entities = FindObjectsOfType<EntityRenderer>();
        SetSortingLayer[] objects = FindObjectsOfType<SetSortingLayer>();

        foreach (var sprite in objects)
        {
            if (sprite.GetComponent<Renderer>() == null && sprite.GetComponent<EntityRenderer>() == null)
            {
                sprite.transform.SetPositionZ(-offset * Array.IndexOf(layers, sprite.sortingLayer) - baseOffset); 
            }
        }

        foreach (var sprite in entities)
        {
            sprite.transform.SetPositionZ(-offset * Array.IndexOf(layers, sprite.SortingLayerName) - baseOffset);
        }

        foreach (var sprite in sprites)
        {
            if (!(sprite.sortingLayerName == "Default" && sprite.GetComponent<SpriteRenderer>() == null))
            {
                sprite.transform.SetPositionZ(-offset * Array.IndexOf(layers, sprite.sortingLayerName) - baseOffset);
            }
        }

        foreach (var audioSource in audioSources)
        {
            audioSource.transform.SetPositionZ(camera.transform.position.z);
        }
    }
}

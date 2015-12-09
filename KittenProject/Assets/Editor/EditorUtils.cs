using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;
using System.Reflection;

public class EditorUtils
{
    public static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public struct Tuple<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Tuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}


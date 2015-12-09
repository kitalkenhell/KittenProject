using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(SetSortingLayer))]
public class SetSortingLayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        string[] options = EditorUtils.GetSortingLayerNames();

        SetSortingLayer setter = (SetSortingLayer) target;
        setter.sortingLayer = options[EditorGUILayout.Popup(ArrayUtility.IndexOf(options, setter.sortingLayer), options)];
    }
}


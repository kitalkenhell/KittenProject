using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

class HistoryWindow : EditorWindow 
{
    List<UnityEngine.Object> history = new List<UnityEngine.Object>();
    Vector2 scrollPosition;
    UnityEngine.Object hoveredObject;

    [MenuItem ("Window/History")]
    public static void  ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(HistoryWindow));
    }

    void OnGUI ()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        hoveredObject = null;

        foreach (UnityEngine.Object obj in history)
        {
            string type = obj.GetType().Name;
            DefaultAsset asset = obj as DefaultAsset;

            type = type.Remove(0, type.IndexOf(".") + 1);

            if (asset != null)
            {
                type = AssetDatabase.GetAssetPath(obj);
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(type, GUILayout.MaxWidth(100));
            GUILayout.Label(obj.name);

            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                hoveredObject = obj;
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        if (Event.current.type == EventType.MouseDown && hoveredObject != null)
        {

                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { hoveredObject };
                DragAndDrop.StartDrag(hoveredObject.name);
                Event.current.Use();
            }


    }

    void OnSelectionChange()
    {
        UnityEngine.Object obj = Selection.activeObject;

        if (obj == null)
        {
            return;
        }

        Repaint();

        if (!history.Contains(obj))
        {
            history.Insert(0, Selection.activeObject);
        }
        else
        {
            history.Remove(obj);
            history.Insert(0, Selection.activeObject);
        }

        Debug.Log("OnSelectionChanged: " + history.Count);
    } 
}
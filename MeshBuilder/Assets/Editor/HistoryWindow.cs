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

            type = type.Remove(0, type.IndexOf(".") + 1);

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

        if (obj == null || obj as DefaultAsset != null)
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
    } 
}
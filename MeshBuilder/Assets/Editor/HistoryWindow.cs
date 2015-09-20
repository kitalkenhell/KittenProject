using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class HistoryWindow : EditorWindow 
{
    List<Object> history = new List<Object>();
    Vector2 scrollPosition;

    [MenuItem ("Window/History")]
    public static void  ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(HistoryWindow));
    }

    void OnGUI ()
    {

        GUIStyle style = new GUIStyle();

        style.active.textColor = Color.white;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (Object obj in history)
        {
            //EditorGUILayout.ObjectField(obj, typeof(Object), true);
            //EditorGUILayout.SelectableLabel(obj.name, GUILayout.Height(17));
            EditorGUILayout.LabelField(obj.name);
        }

        EditorGUILayout.EndScrollView();



        if (Event.current.type == EventType.MouseDown) 
        {
            Debug.Log("Start");
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.objectReferences = new Object[] { history[0] };
            DragAndDrop.StartDrag("Yep");
            Event.current.Use();
        }
    }

    void OnSelectionChange()
    {
        Object obj = Selection.activeObject;

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
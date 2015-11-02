using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

class HistoryWindow : EditorWindow 
{
    class HistoryItem
    {
        public UnityEngine.Object obj;
        public Rect rect;

        public void SetRect(Rect newRect)
        {
            this.rect = newRect;
        }

        public void SetObj(UnityEngine.Object newObj)
        {
            this.obj = newObj;
        }

        public HistoryItem(UnityEngine.Object obj = null)
        {
            this.obj = obj;
            rect = new Rect();
        }
    }

    List<HistoryItem> history = new List<HistoryItem>();
    Vector2 scrollPosition;
    Vector2 lastDragPosition = Vector2.zero;
    UnityEngine.Object hoveredObject;
    bool select = false;

    [MenuItem ("Window/History")]
    public static void  ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(HistoryWindow));
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        hoveredObject = null;
        wantsMouseMove = true;

        history.RemoveAll(item => item.obj == null);

        for (int i = 0; i < history.Count; ++i)
        {
            string type = history[i].obj.GetType().Name;

            GUILayout.BeginHorizontal();
            GUILayout.Label(type, GUILayout.MaxWidth(100));
            GUILayout.Label(history[i].obj.name);
            GUILayout.EndHorizontal();

            if (Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();

                history[i].SetRect(rect);
            }


            if (history[i].rect.Contains(Event.current.mousePosition))
            {
                hoveredObject = history[i].obj;
            }

            if (select && history[i].rect.Contains(lastDragPosition))
            {
                Selection.activeObject = history[i].obj;
                select = false;
            }
        }

        EditorGUILayout.EndScrollView();

        if (Event.current.type == EventType.DragUpdated)
        {
            lastDragPosition = Event.current.mousePosition;
        }
        else if (Event.current.type == EventType.DragExited)
        {
            select = true;
        }
        else
        {
            select = false;
        }

        if (hoveredObject != null)
        {
            if (Event.current.type == EventType.MouseUp)
            {
                Selection.activeGameObject = hoveredObject as GameObject;
            }
            else if (Event.current.type == EventType.MouseDown)
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { hoveredObject };
                DragAndDrop.StartDrag(hoveredObject.name);
            }
        }

        Repaint();
    }

    void OnSelectionChange()
    {
        UnityEngine.Object obj = Selection.activeObject;

        if (obj == null || obj as DefaultAsset != null)
        {
            return;
        }

        Repaint();

        if (!history.Any(item => item.obj == obj))
        {
            HistoryItem item = new HistoryItem(Selection.activeObject);
            history.Insert(0, item);
        }
        else
        {
            //HistoryItem itemToAdd = new HistoryItem(Selection.activeObject);
            //history.RemoveAll(item => item.obj == obj);
            //history.Insert(0, itemToAdd);
        }
    } 
}
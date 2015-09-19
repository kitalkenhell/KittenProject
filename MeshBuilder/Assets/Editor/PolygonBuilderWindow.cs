using UnityEngine;
using UnityEditor;
using System.Collections;

class PolygonBuilderWindow : EditorWindow 
{
    PolygonBuilder polygon;
    int selectedIdx;

    [MenuItem ("Polygon Builder/Vertex editor")]
    public static void  ShowWindow () 
    {
        EditorWindow.GetWindow(typeof(PolygonBuilderWindow));
    }
    
    void OnGUI ()
    {
        if (!isPolygonSelected())
        {
            return;
        }

        if (polygon.selection.Count == 0)
        {

        }
        else
        {
            selectedIdx = polygon.selection[polygon.selection.Count - 1];

            if (polygon.selection.Count == 1)
            {
                if (GUILayout.Button("Apply color to polygon"))
                {
                    for (int i = 0; i < polygon.colors.Length; i++)
                    {
                        polygon.colors[i] = polygon.colors[selectedIdx];
                    }
                }
            }
            else
            {

            }
        }

        polygon.Refresh();
    }

    void SigleSelectionMenu()
    {
        

        

        CommonMenu();
    }

    void CommonMenu()
    {

    }


    bool isPolygonSelected()
    {
        if (Selection.activeGameObject == null)
        {
            return false;
        }

        polygon = Selection.activeGameObject.GetComponent<PolygonBuilder>();

        return polygon != null;
    }

    void OnSelectionChanged()
    {
        //Repaint();
    } 
}
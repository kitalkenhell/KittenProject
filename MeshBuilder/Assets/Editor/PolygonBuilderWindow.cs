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
                VertexColorPicker();
            }
            else
            {
                VertexColorPicker();
            }
        }

        polygon.Refresh();
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    void VertexColorPicker()
    {
        EditorGUILayout.BeginHorizontal();
        Color oldColor = polygon.colors[polygon.selection[0]];
        Color color = EditorGUILayout.ColorField(oldColor);

        if (oldColor != color)
        {
            foreach (var index in polygon.selection)
            {
                polygon.colors[index] = color;
            } 
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Apply color to polygon"))
        {
            for (int i = 0; i < polygon.colors.Count; i++)
            {
                polygon.colors[i] = polygon.colors[selectedIdx];
            }
        }
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

    void OnSelectionChange()
    {
        Repaint();
    } 
}
using UnityEngine;
using UnityEditor;
using System.Collections;

class MeshBuilderWindow : EditorWindow 
{
    MeshBuilder mesh;

    [MenuItem ("Mesh Builder/Vertex editor")]
    public static void  ShowWindow () 
    {
        EditorWindow.GetWindow(typeof(MeshBuilderWindow));
    }
    
    void OnGUI ()
    {
        /*
        Rect position;

        if (!isMeshSelected())
        {
            return;
        }

        if (GUILayout.Button("Apply color to mesh"))
        {
            for (int i = 0; i < mesh.colors.Length; i++)
            {
                mesh.colors[i] = mesh.SelectedColor;
            }
        }

        position = new Rect(0,0,200,100);
        mesh.SelectedColor = EditorGUI.ColorField(position, mesh.SelectedColor);

        */
        mesh.Refresh();
    }

    bool isMeshSelected()
    {
        if (Selection.activeGameObject == null)
        {
            return false;
        }

        mesh = Selection.activeGameObject.GetComponent<MeshBuilder>();

        return mesh != null;
    }
}
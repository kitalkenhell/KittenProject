using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MeshBuilder))]
public class MeshBuilderEditor : Editor
{
    int selectedVertex = -1;

    public override void OnInspectorGUI()
    {
        MeshBuilder builder = (MeshBuilder)target;

        if (GUILayout.Button("Build Quad"))
        {
            builder.BuildQuad();
        }
    }

    public void OnSceneGUI()
    {
        const float buttonSize = 0.3f;
        const float buttonPickSize = 0.5f;

        MeshBuilder builder = (MeshBuilder)target;

        builder.transform.position = Handles.PositionHandle(builder.transform.position, Quaternion.identity);

        for (int i = 0; i < builder.vertices.Length; i++)
        {
            Vector3 position = builder.vertices[i] + builder.transform.position;

            if (i == selectedVertex)
            {
                builder.vertices[i] = Handles.PositionHandle(position, Quaternion.identity) - builder.transform.position;
            }

            bool selected = Handles.Button(position, Quaternion.identity, buttonSize, buttonPickSize, Handles.CircleCap);

            if (selected)
            {
                selectedVertex = i;
            }
        }

        builder.Refresh();
    }
}
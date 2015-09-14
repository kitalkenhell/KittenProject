using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MeshBuilder))]
public class MeshBuilderEditor : Editor
{
    int virtualHandle;
    bool multiselect = false;

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
        const float buttonSize = 0.1f;
        const float buttonPickSize = 0.5f;

        MeshBuilder builder = (MeshBuilder)target;
        

        switch  (Event.current.type)
        {
            case EventType.keyDown:

                if (Event.current.keyCode == (KeyCode.LeftControl))
                {
                    multiselect = true;
                }
                break;

            case EventType.keyUp:

                if (Event.current.keyCode == (KeyCode.LeftControl))
                {
                    multiselect = false;
                }
                break;
        }

        for (int i = 0; i < builder.vertices.Length; i++)
        {
            Vector4 position = builder.transform.TransformPoint(builder.vertices[i]);
            bool selected = Handles.Button(position, Quaternion.identity, buttonSize, buttonPickSize, Handles.DotCap);

            if (selected)
            {
                if (multiselect)
                {
                    builder.selectedVertices.Add(i);
                }
                else
                {
                    builder.selectedVertices.Clear();
                    builder.selectedVertices.Add(i);
                }

                virtualHandle = i;
                return;
            }
        }

        if (builder.selectedVertices.Count == 0)
        {

            Tools.current = UnityEditor.Tool.Move;
            builder.transform.position = Handles.PositionHandle(builder.transform.position, Quaternion.identity);
        }
        else
        {
            Tools.current = UnityEditor.Tool.None;

            Vector4 position = builder.transform.TransformPoint(builder.vertices[virtualHandle]);

            builder.vertices[virtualHandle] = builder.transform.InverseTransformPoint(Handles.PositionHandle(position, Quaternion.identity));

            /*
            for (int i = 0;  i < builder.selectedVertices.Count; ++i)
            {
                builder.vertices[builder.selectedVertices[i]];
            }
             * */
        }

        builder.Refresh();
    }
}
using UnityEngine;
using UnityEditor;
using System.Collections;

class PolygonBuilderWindow : EditorWindow 
{
    PolygonBuilder builder;
    int selectedIdx;

    [MenuItem("Window/Polygon Builder")]
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

        builder.renderMode = (PolygonBuilder.RenderMode)EditorGUILayout.EnumPopup("Render Mode: ", builder.renderMode);

        if (builder.renderMode != PolygonBuilder.RenderMode.vertexColor &&
            builder.renderMode != PolygonBuilder.RenderMode.customMaterial)
        {
            gradientColorPicker();

            if (builder.renderMode == PolygonBuilder.RenderMode.conicalGradient)
            {
                builder.conicalGradientCutoff = EditorGUILayout.FloatField("Cutoff", builder.conicalGradientCutoff);
                builder.conicalGradientMaxAngle = EditorGUILayout.FloatField("Max angle", builder.conicalGradientMaxAngle);
                builder.conicalGradientMaxRange = EditorGUILayout.FloatField("Max range", builder.conicalGradientMaxRange);
            }
        }
        else
        {
            if (builder.selection.Count > 0)
            {
                selectedIdx = builder.selection[builder.selection.Count - 1];
                VertexColorPicker();

            } 
        }
    }

    public void OnInspectorUpdate()
    {
        Repaint();
    }

    void gradientColorPicker()
    {
        for (int i = 0; i < builder.gradientPoints.Count; ++i)
        {
            builder.gradientPoints[i].color = EditorGUILayout.ColorField(builder.gradientPoints[i].color);
        }
    }

    void VertexColorPicker()
    {
        EditorGUILayout.BeginHorizontal();
        Color oldColor = builder.colors[builder.selection[0]];
        Color color = EditorGUILayout.ColorField(oldColor);

        if (oldColor != color)
        {
            foreach (var index in builder.selection)
            {
                builder.colors[index] = color;
            } 
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Apply color to polygon"))
        {
            for (int i = 0; i < builder.colors.Count; i++)
            {
                builder.colors[i] = builder.colors[selectedIdx];
            }
        }
    }


    bool isPolygonSelected()
    {
        if (Selection.activeGameObject == null)
        {
            return false;
        }

        builder = Selection.activeGameObject.GetComponent<PolygonBuilder>();

        return builder != null;
    }

    void OnSelectionChange()
    {
        if (!isPolygonSelected() && builder != null)
        {
            builder.selection.Clear();

            AssetDatabase.SaveAssets();
            Repaint();
        }
    } 
}
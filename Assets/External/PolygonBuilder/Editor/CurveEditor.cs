//using UnityEngine;
//using UnityEditor;
//using System.Collections;

//[CustomEditor(typeof(Curve))]
//public class CurveEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        Curve curve = (Curve)target;

//        if (GUILayout.Button("Reset"))
//        {
//            curve.Reset();
//        }

//        curve.quality = EditorGUILayout.IntField("Quality", curve.quality);
//    }

//    public void OnSceneGUI()
//    {
//        Curve curve = (Curve)target;

//        Handles.color = Color.white;
//        curve.begin = Handles.FreeMoveHandle(curve.begin, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);
//        curve.end = Handles.FreeMoveHandle(curve.end, Quaternion.identity, 0.25f, Vector3.zero, Handles.DotCap);

//        Handles.color = Color.red;
//        curve.tangentBegin = Handles.FreeMoveHandle(curve.begin + curve.tangentBegin, Quaternion.identity, 0.25f, Vector3.one, Handles.DotCap) - curve.begin;

//        curve.tangentEnd = Handles.FreeMoveHandle(curve.end + curve.tangentEnd, Quaternion.identity, 0.25f, Vector3.one, Handles.DotCap) - curve.end;
        
//        if (curve.path.Count >= 2)
//        {
//            Handles.color = Color.white;
//            for (int i = 0; i < curve.path.Count - 1; ++i)
//            {
//                Handles.DrawLine(curve.path[i], curve.path[i + 1]);
//            }
//        }

//        curve.Refresh();
//        SceneView.RepaintAll();
//    }

//}
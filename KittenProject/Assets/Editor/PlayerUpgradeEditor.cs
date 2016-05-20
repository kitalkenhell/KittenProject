﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerUpgradeEditor
{
    [MenuItem("Assets/Create/Doge/PlayerUpgrade")]
    public static void CreateAsset()
    {
        const string defaultPath = "Assets";

        PlayerUpgrade asset = ScriptableObject.CreateInstance<PlayerUpgrade>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = defaultPath;
        }
        else if (!string.IsNullOrEmpty(System.IO.Path.GetExtension(path)))
        {
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(PlayerUpgrade).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
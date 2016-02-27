using UnityEngine;
using UnityEditor;
using System.Collections;

public class PlayerBodySkinEditor
{
    [MenuItem("Assets/Create/Doge/PlayerBodySkin")]
    public static void CreateAsset()
    {
        const string defaultPath = "Assets";

        PlayerBodySkin asset = ScriptableObject.CreateInstance<PlayerBodySkin>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (string.IsNullOrEmpty(path))
        {
            path = defaultPath;
        }
        else if (!string.IsNullOrEmpty(System.IO.Path.GetExtension(path)))
        {
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(PlayerBodySkin).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

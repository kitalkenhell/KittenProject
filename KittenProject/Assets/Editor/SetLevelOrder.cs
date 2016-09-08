using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class SetLevelOrder : MonoBehaviour
{
    [MenuItem("Utility/Set Level Order")]
    static void SetOrder()
    {
        LevelProperties current;
        LevelProperties next;

        LevelSelection levelSelection = GameObject.FindObjectOfType<LevelSelection>();
        List<LevelProperties> allLevels = new List<LevelProperties>();

        string orderGuid = AssetDatabase.FindAssets("t:LevelOrder").First();
        string[] guids = AssetDatabase.FindAssets("t:LevelProperties");

        LevelOrder order = (LevelOrder) AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(orderGuid), typeof(LevelOrder));

        if (levelSelection.levels.Length != order.sceneNames.Length)
        {
            Debug.LogError("levelSelection.levels and order.sceneNames have different sizes");
            return;
        }

        foreach (var guid in guids)
        {
            allLevels.Add((LevelProperties)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(LevelProperties)));
        }

        for (int i = 0; i < levelSelection.levels.Length - 1; ++i)
        {
            current = GetLevelWithScene(order.sceneNames[i], allLevels);
            next = GetLevelWithScene(order.sceneNames[i + 1], allLevels);

            current.nextLevel = next;
            EditorUtility.SetDirty(current);

            levelSelection.levels[i].levelProperties = current;
        }

        current = GetLevelWithScene(order.sceneNames.Last(), allLevels);
        next = GetLevelWithScene(order.sceneNames.First(), allLevels);

        current.nextLevel = next;
        EditorUtility.SetDirty(current);

        levelSelection.levels.Last().levelProperties = current;

        Debug.Log("SetOrder finished!");
    }

    static LevelProperties GetLevelWithScene(string sceneName, List<LevelProperties> levels)
    {
        foreach (var level in levels)
        {
            if (level.sceneName == sceneName)
            {
                return level;
            }
        }

        Debug.LogError("Can't find level: " + sceneName);

        return null;
    }
}

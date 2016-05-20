using UnityEngine;
using System.Collections;
using UnityEditor;

public class DebugOptions : MonoBehaviour
{
    const int freeMoneyAmount = 500;

    [MenuItem("Utility/Add Money")]
    static void AddMoney()
    {
        PersistentData.Coins += freeMoneyAmount;
        PostOffice.PostAmountOfCoinsChanged();
    }

    [MenuItem("Utility/Reset Money")]
    static void ResetMoney()
    {
        PersistentData.Coins = 0;
        PostOffice.PostAmountOfCoinsChanged();
    }

    [MenuItem("Utility/Unlock All Levels")]
    static void UnlockAllLevelsMoney()
    {
        string[] guids = AssetDatabase.FindAssets("t:LevelProperties");

        foreach (var guid in guids)
        {
            LevelProperties level = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(LevelProperties)) as LevelProperties;

            if (level != null)
            {
                level.IsLocked = false;
            }
        }
    }
}

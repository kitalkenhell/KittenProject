using UnityEngine;
using UnityEditor;

public class ResetPrefs : MonoBehaviour
{
    [MenuItem("Utility/Reset PlayerPrefs")]
    static void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}

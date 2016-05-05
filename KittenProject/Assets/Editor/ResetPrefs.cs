using UnityEngine;
using UnityEditor;

public class ResetPrefs : MonoBehaviour 
{
    [MenuItem("Ulility/Reset PlayerPrefs")]
    static void Sort()
    {
        PlayerPrefs.DeleteAll();
    }
}

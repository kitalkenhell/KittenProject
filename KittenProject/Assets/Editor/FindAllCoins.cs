using UnityEngine;
using System.Collections;
using UnityEditor;

public class FindAllCoins : MonoBehaviour
{
    [MenuItem("Utility/Print coins count")]
    static void FindALlCoins()
    {
        Pickup[] pickups = GameObject.FindObjectsOfType<Pickup>();

        int count = 0;

        foreach (var pickup in pickups)
        {
            if (pickup.type == Pickup.Type.coin)
            {
                ++count;
            }
        }

        Debug.Log("Amount of all coins: " + count);
    }
}

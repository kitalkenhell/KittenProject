using UnityEngine;

class Utils
{
    public static float RandomSign()
    {
        return Random.value < 0.5f ? -1.0f : 1.0f;
    }
}


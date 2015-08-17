using UnityEngine;
using System.Collections;

public static class TransformExtensions
{
    public static void SetPositionX(this Transform t, float x)
    {
        t.position = new Vector3(x, t.position.y, t.position.z);
    }

    public static void SetPositionY(this Transform t, float y)
    {
        t.position = new Vector3(t.position.x, y, t.position.z);
    }

    public static void SetPositionZ(this Transform t, float z)
    {
        t.position = new Vector3(t.position.x, t.position.y, z);
    }
}
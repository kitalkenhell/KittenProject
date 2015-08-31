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

    public static void SetPositionXy(this Transform t, float x, float y)
    {
        t.position = new Vector3(x, y, t.position.z);
    }

    public static void SetPositionXz(this Transform t, float x, float z)
    {
        t.position = new Vector3(x, t.position.y, z);
    }

    public static void SetPositionYz(this Transform t, float y, float z)
    {
        t.position = new Vector3(t.position.x, y, z);
    }

    public static void SetPositionXy(this Transform t, Vector2 v)
    {
        t.position = new Vector3(v.x, v.y, t.position.z);
    }

    public static void SetPositionXz(this Transform t, Vector2 v)
    {
        t.position = new Vector3(v.x, t.position.y, v.y);
    }

    public static void SetPositionYz(this Transform t, Vector2 v)
    {
        t.position = new Vector3(t.position.x, v.x, v.y);
    }
}
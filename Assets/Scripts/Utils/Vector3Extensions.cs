using UnityEngine;
using System.Collections;

public static class Vector3Extensions
{
    public static Vector2 XX(this Vector3 v)
    {
        return new Vector2(v.x, v.x);
    }

    public static Vector2 XY(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 XZ(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 YX(this Vector3 v)
    {
        return new Vector2(v.y, v.x);
    }

    public static Vector2 YY(this Vector3 v)
    {
        return new Vector2(v.y, v.y);
    }

    public static Vector2 YZ(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector2 ZX(this Vector3 v)
    {
        return new Vector2(v.z, v.x);
    }

    public static Vector2 ZY(this Vector3 v)
    {
        return new Vector2(v.z, v.y);
    }

    public static Vector2 ZZ(this Vector3 v)
    {
        return new Vector2(v.z, v.z);
    }

    public static void XY(this Vector3 v, Vector2 xy)
    {
        v.x = xy.x;
        v.y = xy.y;
    }

    public static void XZ(this Vector3 v, Vector2 xz)
    {
        v.x = xz.x;
        v.z = xz.y;
    }

    public static void YZ(this Vector3 v, Vector2 yz)
    {
        v.y = yz.x;
        v.z = yz.y;
    }

}
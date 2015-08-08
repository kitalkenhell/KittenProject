using UnityEngine;
using System.Collections;

public static class Vector3Extensions
{
    public static Vector2 xx(this Vector3 v)
    {
        return new Vector2(v.x, v.x);
    }

    public static Vector2 xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 xz(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector2 yx(this Vector3 v)
    {
        return new Vector2(v.y, v.x);
    }

    public static Vector2 yy(this Vector3 v)
    {
        return new Vector2(v.y, v.y);
    }

    public static Vector2 yz(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector2 zx(this Vector3 v)
    {
        return new Vector2(v.z, v.x);
    }

    public static Vector2 zy(this Vector3 v)
    {
        return new Vector2(v.z, v.y);
    }

    public static Vector2 zz(this Vector3 v)
    {
        return new Vector2(v.z, v.z);
    }

    public static void xy(this Vector3 v, Vector2 xy)
    {
        v.x = xy.x;
        v.y = xy.y;
    }

    public static void xz(this Vector3 v, Vector2 xz)
    {
        v.x = xz.x;
        v.z = xz.y;
    }

    public static void yz(this Vector3 v, Vector2 yz)
    {
        v.y = yz.x;
        v.z = yz.y;
    }

}
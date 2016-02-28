using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public static class TransformExtensions
{
    public static void Reset(this Transform t)
    {
        t.position = Vector3.zero;
        t.rotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

    public static void ResetLocal(this Transform t)
    {
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
    }

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

    public static void SetPositionXY(this Transform t, float x, float y)
    {
        t.position = new Vector3(x, y, t.position.z);
    }

    public static void SetPositionXZ(this Transform t, float x, float z)
    {
        t.position = new Vector3(x, t.position.y, z);
    }

    public static void SetPositionYZ(this Transform t, float y, float z)
    {
        t.position = new Vector3(t.position.x, y, z);
    }

    public static void SetPositionXY(this Transform t, Vector2 v)
    {
        t.position = new Vector3(v.x, v.y, t.position.z);
    }

    public static void SetPositionXZ(this Transform t, Vector2 v)
    {
        t.position = new Vector3(v.x, t.position.y, v.y);
    }

    public static void SetPositionYZ(this Transform t, Vector2 v)
    {
        t.position = new Vector3(t.position.x, v.x, v.y);
    }

    public static void SetScaleX(this Transform t, float x)
    {
        t.localScale = new Vector3(x, t.localScale.y, t.localScale.z);
    }

    public static void SetScaleY(this Transform t, float y)
    {
        t.localScale = new Vector3(t.localScale.x, y, t.localScale.z);
    }

    public static void SetScaleZ(this Transform t, float z)
    {
        t.localScale = new Vector3(t.localScale.x, t.localScale.y, z);
    }

    public static void SetScaleXY(this Transform t, float x, float y)
    {
        t.localScale = new Vector3(x, y, t.localScale.z);
    }

    public static void SetScaleXZ(this Transform t, float x, float z)
    {
        t.localScale = new Vector3(x, t.localScale.y, z);
    }

    public static void SetScaleYZ(this Transform t, float y, float z)
    {
        t.localScale = new Vector3(t.localScale.x, y, z);
    }

    public static void SetScaleXY(this Transform t, Vector2 v)
    {
        t.localScale = new Vector3(v.x, v.y, t.localScale.z);
    }

    public static void SetScaleXZ(this Transform t, Vector2 v)
    {
        t.localScale = new Vector3(v.x, t.localScale.y, v.y);
    }

    public static void SetScaleYZ(this Transform t, Vector2 v)
    {
        t.localScale = new Vector3(t.localScale.x, v.x, v.y);
    }

    public static void SetRotationX(this Transform t, float x)
    {
        t.eulerAngles = new Vector3(x, t.eulerAngles.y, t.eulerAngles.z);
    }

    public static void SetRotationY(this Transform t, float y)
    {
        t.eulerAngles = new Vector3(t.eulerAngles.x, y, t.eulerAngles.z);
    }

    public static void SetRotationZ(this Transform t, float z)
    {
        t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, z);
    }

    public static void FlipX(this Transform t)
    {
        t.localScale = new Vector3(t.localScale.x * -1, t.localScale.y, t.localScale.z);
    }

    public static void FlipY(this Transform t)
    {
        t.localScale = new Vector3(t.localScale.x, t.localScale.y * -1, t.localScale.z);
    }

    public static void FlipZ(this Transform t)
    {
        t.localScale = new Vector3(t.localScale.x, t.localScale.y, t.localScale.z * -1);
    }
}
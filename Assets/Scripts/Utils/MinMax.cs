using System;
using UnityEngine;


[System.Serializable]
public struct MinMax
{
    public float min;
    public float max;
}

[System.Serializable]
public struct MinMaxVec3
{
    public Vector3 min;
    public Vector3 max;
}

public struct MinMax<T>
{
    public T min;
    public T max;
}

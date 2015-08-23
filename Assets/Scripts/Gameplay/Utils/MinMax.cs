using System;
using UnityEngine;


[System.Serializable]
public struct MinMax
{
    public float min;
    public float max;
}

public struct MinMax<T>
{
    public T min;
    public T max;
}

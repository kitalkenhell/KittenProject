using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class CollectionsExtensions
{
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static T First<T>(this List<T> list)
    {
        return list[0];
    }

    public static int LastIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }


    public static T Last<T>(this T[] array)
    {
        return array[array.Length - 1];
    }

    public static T First<T>(this T[] array)
    {
        return array[0];
    }

    public static int LastIndex<T>(this T[] array)
    {
        return array.Length - 1;
    }

}
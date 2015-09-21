using UnityEngine;
using System.Collections;

public static class Utils
{
    public struct Tuple<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Tuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }
}

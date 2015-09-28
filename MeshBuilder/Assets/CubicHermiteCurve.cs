using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubicHermiteCurve : MonoBehaviour
{
    public Vector2 begin = Vector2.zero;
    public Vector2 end = Vector2.one;
    public Vector2 tangentBegin = Vector2.left;
    public Vector2 tangentEnd = Vector2.right;

    public List<Vector2> path;

    public void Refresh()
    {
        path.Clear();
        path.Add(begin);
        int steps = 20;

        for (int t = 0; t < steps; t++)
        {
            float s = (float)t / (float)steps;   
            float h1 = 2 * s * s * s - 3 * s * s + 1;          
            float h2 = -2 * s * s * s + 3 * s * s;             
            float h3 = s * s * s - 2 * s * s * s + s;         
            float h4 = s * s * s - s * s;
            Vector2 p = h1 * begin +  h2 * end + h3 * tangentBegin + h4 * tangentEnd;

            path.Add(p);
        }
    }

    public void Reset()
    {
        begin = Vector2.zero;
        end = Vector2.one;
        tangentBegin = Vector2.left;
        tangentEnd = Vector2.right;
    }
}

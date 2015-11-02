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
        
        int steps = 500;

        for (int t = 0; t < steps; t++)
        {
            float s = (float)t / (float)steps;   
            float h1 = 2 * pow3(s) - 3 * pow2(s) + 1;          
            float h2 = -2 * pow3(s) + 3 * pow2(s);
            float h3 = pow3(s) - 2 * pow2(s) + s;         
            float h4 = pow3(s) - pow2(s);
            Vector2 p = h1 * begin + h2 * end + h3 * tangentBegin * 3 + h4 * -tangentEnd * 3;

            path.Add(p);
        }
        path.Add(end);
    }

    public void Reset()
    {
        begin = Vector2.zero;
        end = Vector2.one;
        tangentBegin = Vector2.left;
        tangentEnd = Vector2.right;
    }

    float pow3(float value)
    {
        return value * value * value;
    }

    float pow2(float value)
    {
        return value * value;
    }
}

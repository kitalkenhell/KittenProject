using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Curve
{
    public Vector3 begin = Vector3.zero;
    public Vector3 end = Vector3.up;
    public Vector3 tangentBegin = Vector3.left;
    public Vector3 tangentEnd = Vector3.right;
    public int quality = 15;

    public float lenght;
    public List<Vector3> path = new List<Vector3>();
    public List<float> additiveDistance = new List<float>();

    public void Refresh()
    {
        path.Clear();
        additiveDistance.Clear();

        path.Add(begin);
        lenght = 0;
        
        for (int i = 0; i < quality; i++)
        {
            path.Add(Evaluate((float)i / (float)quality));
            lenght += Vector3.Distance(path[i], path[i + 1]);
            additiveDistance.Add(lenght);
        }

        path.Add(end);
        lenght += Vector3.Distance(path[path.Count - 1], path[path.Count - 2]);
        additiveDistance.Add(lenght);
    }

    public void Reset()
    {
        begin = Vector3.zero;
        end = Vector3.up;
        tangentBegin = Vector3.left;
        tangentEnd = Vector3.right;
    }

    public Vector3 PointOnCurveSmooth(float distance)
    {

        if (distance > lenght)
        {
            return end;
        }
        else if (distance <= 0)
        {
            return begin;
        }

        int index = additiveDistance.BinarySearch(distance);

        if (index < 0)
        {
            index = ~index;
        }

        float distanceToGo = additiveDistance[index] - distance;

        return Evaluate(1 - (additiveDistance[index] / lenght - distanceToGo / lenght));
    }

    public Vector3 PointOnCurve(float distance)
    {
        return Evaluate(1 - distance / lenght);
    }

    public Vector3 Evaluate(float value)
    {
        float h1 = 2 * Pow3(value) - 3 * Pow2(value) + 1;
        float h2 = -2 * Pow3(value) + 3 * Pow2(value);
        float h3 = Pow3(value) - 2 * Pow2(value) + value;
        float h4 = Pow3(value) - Pow2(value);
        return h1 * begin + h2 * end + h3 * tangentBegin * 3 + h4 * -tangentEnd * 3;
    }

    float Pow3(float value)
    {
        return value * value * value;
    }

    float Pow2(float value)
    {
        return value * value;
    }
}

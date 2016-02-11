using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Curve
{
    public Vector3 begin = Vector3.zero;
    public Vector3 end = Vector3.up;
    public Vector3 beginTangent = Vector3.left;
    public Vector3 endTangent = Vector3.right;
    public int quality = 15;

    public float lenght;
    public List<Vector3> path = new List<Vector3>();
    public List<float> additiveDistance = new List<float>();

    public void Refresh()
    {
        path.Clear();
        additiveDistance.Clear();

        lenght = 0;
        path.Add(begin);
        additiveDistance.Add(lenght);

        for (int i = 1; i <= quality; i++)
        {
            path.Add(Evaluate((float)i / (float)quality));
            lenght += Vector3.Distance(path[i], path[i - 1]);
            additiveDistance.Add(lenght);
        }
    }

    public void Reset()
    {
        begin = Vector3.zero;
        end = Vector3.up;
        beginTangent = Vector3.left;
        endTangent = Vector3.right;
    }

    public Vector3 PointOnCurve(float distance)
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

        float weight = (additiveDistance[index] - distance) / (additiveDistance[index] - additiveDistance[index - 1]);

        return path[index - 1] * weight + path[index] * (1 - weight);
    }

    public Vector3 PointOnCurveFast(float distance)
    {
        return Evaluate(1 - distance / lenght);
    }

    public Vector3 Evaluate(float value)
    {
        float h1 = 2 * Pow3(value) - 3 * Pow2(value) + 1;
        float h2 = -2 * Pow3(value) + 3 * Pow2(value);
        float h3 = Pow3(value) - 2 * Pow2(value) + value;
        float h4 = Pow3(value) - Pow2(value);
        return h1 * begin + h2 * end + h3 * beginTangent * 3 + h4 * -endTangent * 3;
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

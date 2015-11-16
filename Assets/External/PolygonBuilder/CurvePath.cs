using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurvePath : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public List<Curve> curves = new List<Curve>();
    public int quality = 20;
    public float length;

    float find = 0;
    public Vector3 handle;

    public void Refresh()
    {
        find += Time.deltaTime * 100;

        if (find > 1)
            find = 0;

        length = 0;

        foreach (Curve curve in curves)
        {
            length += curve.lenght;
        }

        handle = PointOnPath(find * length);
    }

    public void Reset()
    {
        Curve curve = new Curve();

        points.Clear();
        curves.Clear();

        curve.begin = Vector3.zero;
        curve.end = Vector3.up;
        curve.tangentBegin = Vector3.left;
        curve.tangentEnd = Vector3.right;

        points.Add(curve.begin);
        points.Add(curve.end);
        curves.Add(curve);
    }

    public void AddPoint()
    {
        if (points.Count >= 2)
        {
            Curve curve = new Curve();

            curve.begin = points[points.Count - 1];
            curve.end = points[points.Count - 1] + points[points.Count - 1] - points[points.Count - 2];
            curve.tangentBegin = Vector3.left;
            curve.tangentEnd = Vector3.right;

            points.Add(curve.end);
            curves.Add(curve);
        }
    }

    public Vector3 PointOnPath(float distance, int startingPoint = 0)
    {
        float distanceToGo = distance;
        int index = 0;

        if (distance >= length)
        {
            return points[points.Count - 1];
        }
        else if (distance <= 0)
        {
            return points[index];
        }

        while (distanceToGo > 0)
        {
            distanceToGo -= curves[index].lenght;
            ++index;
        }
        --index;

        Debug.Log(distance / length + " " + index);
        return curves[index].PointOnCurve(Mathf.Abs(distanceToGo));
    }
}

using UnityEngine;
using System.Collections;

public class PlaceObjectsAlongCurve : MonoBehaviour
{
    public GameObject sourceObject;
    public CurvePath curve;
    public int amount;
    public bool useCurveFastApproximation;

    public void Spawn()
    {
        RemoveAll();

        for (int i = 0; i < amount; ++i)
        {
            Vector3 position = curve.PointOnPathWorld((float) i / (amount - 1) * curve.length, useCurveFastApproximation);

            GameObject clone = Instantiate(sourceObject, position, Quaternion.identity) as GameObject;
            clone.transform.parent = transform;
        }
    }

    public void RemoveAll()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

}

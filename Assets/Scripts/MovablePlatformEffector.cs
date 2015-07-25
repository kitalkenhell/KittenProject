using UnityEngine;
using System.Collections;

public class MovablePlatformEffector : MonoBehaviour
{
    public delegate void OnMoved(Vector3 displacement);
    public OnMoved onMoved;
}

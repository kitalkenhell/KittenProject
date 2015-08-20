using UnityEngine;
using System.Collections;

public class MovablePlatformEffector : MonoBehaviour
{
    public delegate void OnMoved(Vector3 displacement);
    public delegate void Notification();
    public OnMoved onMoved;
    public Notification onEnter;
    public Notification onExit;
}

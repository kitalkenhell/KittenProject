using UnityEngine;
using System.Collections;

public class SetWaypointOnTrigger : MonoBehaviour
{
    public int waypointIndex;
    public MoveAlongWaypoints target;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        target.currentWaypoint = waypointIndex;
    }
}

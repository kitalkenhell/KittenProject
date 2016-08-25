using UnityEngine;
using System.Collections;

public class DragonChasePlayer : MonoBehaviour
{
    public float maxDistanceFromPlayer;

    MoveAlongWaypoints mover;

    void Start()
    {
        mover = GetComponent<MoveAlongWaypoints>();
    }

    void Update()
    {
        mover.Pause(transform.position.x + maxDistanceFromPlayer > CoreLevelObjects.player.transform.position.x);
    }
}

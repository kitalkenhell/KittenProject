using UnityEngine;
using System.Collections;

public class MovableSpike : MovablePlatform 
{
    public float normalDirectionForce;
    public Vector3 additionalForce;
    public float disableControlsDuration;
    public int damage;

    bool active;

    new void Start()
    {
        base.Start();
        active = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && active)
        {
            active = false;
            player.Hit(damage);
            player.Push((other.transform.position - transform.position).normalized * normalDirectionForce + additionalForce, true, true, disableControlsDuration);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        active = true;
    }
}  
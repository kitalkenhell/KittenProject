using UnityEngine;
using System.Collections;

public class PushPlayerOnTriggerEnter : MonoBehaviour
{
    public float pushingForce;
    public float pushingForceUpAmplification;
    public float disableControlsDuration;
    public bool dealDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Vector2 force = (player.transform.position - transform.position).normalized;
            force.y = Mathf.Max(pushingForceUpAmplification, force.y);

            if (dealDamage)
            {
                player.PushAndHit(force.normalized * pushingForce, true, true, disableControlsDuration);
            }
            else
            {
                player.Push(force.normalized * pushingForce, true, true, disableControlsDuration);
            }
        }
    }
}

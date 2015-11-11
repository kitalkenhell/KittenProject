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
            player.Push((player.transform.position - transform.position + Vector3.up * pushingForceUpAmplification).normalized * pushingForce,
                true, true, disableControlsDuration);

            if (dealDamage)
            {
                player.Hit(); 
            }
        }
    }
}

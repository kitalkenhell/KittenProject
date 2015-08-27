using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour
{
    public Transform force;
    public float disableControlsDuration;
    public int damage;

    bool active;

    void Start()
    {
        active = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        const float amplification = 3.0f;

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && active)
        {
            active = false;
            player.Hit(damage);
            player.Push((force.position - transform.position).xy() * amplification, true, true, disableControlsDuration);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        active = true;
    }

}

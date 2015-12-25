using UnityEngine;
using System.Collections;

public class VictoryTrigger : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null && player.IsGrounded)
        {
            GetComponent<Collider2D>().enabled = false;
            PostOffice.PostVictory();
        }
    }
}

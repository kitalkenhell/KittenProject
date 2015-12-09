using UnityEngine;
using System.Collections;

public class KnightsSword : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.PushAndHit(Vector2.up*30);
        }
    }
}

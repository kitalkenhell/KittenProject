using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseDefeatedDragonTrigger : MonoBehaviour
{
    public DefeatedDragon dragon;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        dragon.MoveToTheEnd = true;
        Destroy(gameObject);
    }
}

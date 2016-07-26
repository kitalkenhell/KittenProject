using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour 
{
    public float speed;
    public float acceleration;

    BoxCollider2D boxCollider;
    float counter = 0;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        MeshRenderer[] windRenderers = GetComponentsInChildren<MeshRenderer>();

        counter = 0;

        for (int i = 0; i < windRenderers.Length; ++i)
        {
            windRenderers[i].material.SetFloat("_TimeOffset", Random.value * i);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            ++counter;
            player.EnterWindZone(speed, acceleration, boxCollider.bounds.max.y);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            --counter;
            if (counter <= 0)
            {
                player.ExitWindZone(); 
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour 
{
    public float speed;
    public float acceleration;
    public Color gizmoColor;

    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.EnterWindZone(speed, acceleration, boxCollider.bounds.max.y);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ExitWindZone();
        }
    }

    void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider != null)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, 1.0f));
        }
    }
}

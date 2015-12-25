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
        MeshRenderer[] windRenderers = GetComponentsInChildren<MeshRenderer>();

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
            Gizmos.DrawCube(transform.position + boxCollider.offset.Vec3() , 
                new Vector3(boxCollider.size.x * transform.lossyScale.x, boxCollider.size.y * transform.lossyScale.y, 1.0f));
        }
    }
}

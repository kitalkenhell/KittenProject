using UnityEngine;
using System.Collections;

public class DestructiblePlatform : MonoBehaviour
{
    public MinMax angularSpeed;
    public MinMax verticalForce;
    public MinMax horizontalForce;
    public float initialDelay;
    public float durability;
    public int chunksDetachedBeforeColapse;

    public GameObject platform;
    public Rigidbody2D[] chunks;

    BoxCollider2D boxCollider;

    int detachedChunks;
    float detachTimer;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        detachTimer = initialDelay;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player && other.GetComponent<PlayerController>().Velocity.y <= 0)
        {
            detachTimer -= Time.fixedDeltaTime;

            if (detachTimer < 0)
            {
                detachTimer += durability;
                DetachChunk(chunks[detachedChunks++]);
            }

            if (detachedChunks > chunksDetachedBeforeColapse)
            {
                platform.SetActive(false);

                foreach (var chunk in chunks)
                {
                    DetachChunk(chunk);
                }
                boxCollider.enabled = false;
            }  
        }
    }

    void DetachChunk(Rigidbody2D chunk)
    {
        chunk.isKinematic = false;
        chunk.GetComponent<Collider2D>().enabled = true;
        chunk.angularVelocity = angularSpeed.Random() * Utils.RandomSign();
        chunk.velocity = new Vector2(horizontalForce.Random() * Utils.RandomSign(), verticalForce.Random());
    }
}

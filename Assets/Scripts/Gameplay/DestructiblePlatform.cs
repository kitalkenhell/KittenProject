using UnityEngine;
using System.Collections;

public class DestructiblePlatform : MonoBehaviour
{
    public MinMax angularSpeed;
    public MinMax verticalForce;
    public MinMax horizontalForce;
    public float durability;
    public int chunksDetachedBeforeColapse;

    public GameObject platform;
    public Rigidbody2D[] chunks;

    BoxCollider2D boxCollider;

    int detachedChunks;
    float detachTimer;
    float detachInterval;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        detachInterval = (durability - (durability / chunksDetachedBeforeColapse) * 2) / chunksDetachedBeforeColapse;
        detachTimer = detachInterval * 2;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        durability -= Time.fixedDeltaTime;
        detachTimer -= Time.fixedDeltaTime;

        if (detachTimer < 0)
        {
            detachTimer += detachInterval;
            DetachChunk(chunks[detachedChunks++]);
        }

        if (durability < 0)
        {
            platform.SetActive(false);

            foreach (var chunk in chunks)
            {
                DetachChunk(chunk);
            }
            boxCollider.enabled = false;
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

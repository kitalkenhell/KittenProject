using UnityEngine;
using System.Collections;

public class DestructiblePlatform : MonoBehaviour
{
    public MinMax angularSpeed;
    public MinMax verticalForce;
    public MinMax horizontalForce;
    public float durability;

    public GameObject platform;
    public Rigidbody2D[] chunks;

    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        durability -= Time.fixedDeltaTime;

        if (durability < 0)
        {
            platform.SetActive(false);

            foreach (var chunk in chunks)
            {
                chunk.gameObject.SetActive(true);
                chunk.angularVelocity = Random.Range(angularSpeed.min, angularSpeed.max) * Utils.RandomSign();
                chunk.velocity = new Vector2(Random.Range(horizontalForce.min, horizontalForce.max) * Utils.RandomSign(), 
                    Random.Range(verticalForce.min, verticalForce.max));
            }
            boxCollider.enabled = false;
        } 
    }
}

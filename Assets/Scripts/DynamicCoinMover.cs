using UnityEngine;
using System.Collections;

public class DynamicCoinMover : MonoBehaviour
{
    public float speed;
    public float shrinkingSpeed;
    public Transform target;
    public Rigidbody2D body;

    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        body = GetComponent<Rigidbody2D>();

        body.isKinematic = true;
    }

    void Update()
    {
        print("move: " + transform.position);

        const float scaleThreshold = 0.5f;

        body.MovePosition(Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime).xy());
        transform.localScale -= Vector3.one * shrinkingSpeed * Time.deltaTime;

        if (transform.localScale.x < scaleThreshold)
        {
            Destroy(gameObject);
        }

    }
}

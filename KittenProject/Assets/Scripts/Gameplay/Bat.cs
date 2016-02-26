using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour
{
    enum State
    {
        chase,
        flyBack,
        wait
    }

    public float velocity;
    public float startChaseDistance;
    public float maxChaseDistance;
    public float disableDistance;
    public AudioSource wingFlapSound;

    Rigidbody2D body;

    Transform target;
    Vector3 startingPosition;
    State state;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        target = CoreLevelObjects.player.transform;
    }

    void OnEnable()
    {
        startingPosition = transform.position;
        state = State.wait;
    }

    void Update()
    {
        if (state == State.chase)
        {
            body.MovePosition(transform.position + (target.position - transform.position).normalized * velocity * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) > maxChaseDistance)
            {
                state = State.flyBack;
            }
        }
        else if (state == State.flyBack)
        {
            const float distanceThreshold = 2.0f;

            body.MovePosition(transform.position + (startingPosition - transform.position).normalized * velocity * Time.deltaTime);

            if (Vector2.Distance(transform.position, startingPosition) < distanceThreshold)
            {
                state = State.wait;
            }
        }
        else
        {
            float distance = Vector2.Distance(transform.position, target.position);

            if (distance > disableDistance)
            {
                gameObject.SetActive(false);
            }
            else if (distance < startChaseDistance)
            {
                state = State.chase;
            }
        }

    }

    void FlapWing()
    {
        wingFlapSound.Play();
    }
}

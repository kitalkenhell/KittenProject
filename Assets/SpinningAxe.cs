using UnityEngine;
using System.Collections;

public class SpinningAxe : MonoBehaviour 
{
    public AnimationCurve positionCurve;
    public float maxAngle = 45.0f;
    public float frequency = 1.0f;
    public float force = 20.0f;

    Rigidbody2D body;
    float speed = 0;
    float lastRotation = 0;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.Push(new Vector2(force * speed, Mathf.Abs(force * speed)));
        }
    }

    void Update()
    {
        float time = Time.time * frequency;
        float currentRotiation; 

        time = time - Mathf.Floor(time);
        currentRotiation = positionCurve.Evaluate(time) * maxAngle;
        speed = currentRotiation - lastRotation;
        lastRotation = currentRotiation;

        body.MoveRotation(currentRotiation);
    }
}

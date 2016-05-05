using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour
{
    public float acceleration;
    public float deaccelerationSpeedFactor;
    public float minDeacceleration;
    public float sensitivity;
    public float bounciness;
    public MinMax position;

    float lastMousePosition;
    float speed;

    void Update()
    {
        int touchIndex = 0;
        int leftButton = 0;

        if (Input.touchCount > touchIndex && Input.GetTouch(touchIndex).phase == TouchPhase.Moved)
        {
            speed = Mathf.MoveTowards(speed, Input.GetTouch(touchIndex).deltaPosition.x * sensitivity, acceleration * Time.deltaTime);
        }
        else if (Input.GetMouseButton(leftButton))
        {
            speed = Mathf.MoveTowards(speed, (Input.mousePosition.x - lastMousePosition) * sensitivity, acceleration * Time.deltaTime);
            lastMousePosition = Input.mousePosition.x;
        }
        else
        {
            lastMousePosition = Input.mousePosition.x;
            speed = Mathf.MoveTowards(speed, 0, Mathf.Max(Mathf.Abs(speed * deaccelerationSpeedFactor * Time.deltaTime), minDeacceleration));
        }

        transform.Translate(-speed * Time.deltaTime, 0, 0);

        if (transform.position.x < position.min)
        {
            transform.SetPositionX(position.min);
            speed = -speed * bounciness;
        }

        if (transform.position.x > position.max)
        {
            transform.SetPositionX(position.max);
            speed = -speed * bounciness;
        }
    }
}

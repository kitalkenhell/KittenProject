using UnityEngine;
using System.Collections;

public class SetHellfireOnTriggerEneter : MonoBehaviour
{
    public Hellfire hellfire;
    public bool setSpeed;
    public bool setPosition;
    public bool allowToMoveBack;
    public float speed;
    public float position;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            if (setSpeed)
            {
                hellfire.speed = speed;
            }
            if (setPosition && (allowToMoveBack ||hellfire.transform.position.y < position))
            {
                hellfire.transform.SetPositionY(position);
            }
        }
    }
}

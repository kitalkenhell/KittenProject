using UnityEngine;
using System.Collections;

public class SpriteTurner : MonoBehaviour
{
    public float speed;
    public bool mirrorRotations;

    public bool Turning
    {
        get;
        private set;
    }

    void Awake()
    {
        Turning = false;
    }

    public void Turn()
    {
        if (!Turning)
        {
            Turning = true;
            StartCoroutine(TurnCoroutine());
        }
    }

    public void InstantTurn()
    {
        if (mirrorRotations)
        {
            transform.SetRotationX(360 - transform.eulerAngles.x);
            transform.SetRotationZ(360 - transform.eulerAngles.z);
        }

        transform.SetRotationY(Mathf.Approximately(transform.rotation.eulerAngles.y, 0) ? 180.0f : 0.0f);
    }

    IEnumerator TurnCoroutine()
    {
        float targetRotation = Mathf.Approximately(transform.rotation.eulerAngles.y, 0) ? 180.0f : 0.0f;

        if (mirrorRotations)
        {
            transform.SetRotationX(360 - transform.eulerAngles.x);
            transform.SetRotationZ(360 - transform.eulerAngles.z); 
        }

        while (!Mathf.Approximately(transform.rotation.eulerAngles.y, targetRotation))
        {
            transform.SetRotationY(Mathf.MoveTowards(transform.eulerAngles.y, targetRotation, speed * Time.deltaTime));

            yield return null;
        }

        Turning = false;
    }
}


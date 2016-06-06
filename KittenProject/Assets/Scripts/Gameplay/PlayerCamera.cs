using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour 
{
    public Transform target;
    public Vector3 offset;
    public Vector3 offsetAfterVictory;
    public float changingCameraOffsetSpeed;

    public Transform hellfire;
    public float minDistanceFromHellfire;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        PostOffice.victory += OnVictory;

#if UNITY_EDITOR
        Camera camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Color;
#endif
    }

    void OnDestroy()
    {
        PostOffice.victory -= OnVictory;
    }

    void OnVictory()
    {
        animator.enabled = true;
        StartCoroutine(ChangeOffset(offsetAfterVictory));
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;

        if (hellfire != null)
        {
            if (transform.position.y - minDistanceFromHellfire < hellfire.position.y)
            {
                transform.SetPositionY(hellfire.position.y + minDistanceFromHellfire);
            } 
        }
	}

    IEnumerator ChangeOffset(Vector3 targetOffset)
    {
        while (Vector3.SqrMagnitude(offset - offsetAfterVictory) > Mathf.Epsilon)
        {
            offset = Vector3.MoveTowards(offset, targetOffset, changingCameraOffsetSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

using UnityEngine;
using System.Collections;

public class RotateZAxis : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.SetRotationZ(transform.rotation.eulerAngles.z + speed * Time.deltaTime);
    }
}

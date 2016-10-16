using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepLocalXScale : MonoBehaviour
{
    float correctSign;

    void Start()
    {
        correctSign = Mathf.Sign(transform.lossyScale.x);
    }

    void Update()
    {
        if (Mathf.Sign(transform.lossyScale.x) != correctSign)
        {
            transform.FlipX();
        }
    }
}

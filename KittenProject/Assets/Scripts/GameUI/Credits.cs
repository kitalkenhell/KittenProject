using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour
{
    public event System.Action onFinished;

    public void OnFinished()
    {
        if (onFinished != null)
        {
            onFinished();
        }
    }
}

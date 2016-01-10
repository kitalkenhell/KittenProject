using UnityEngine;
using System.Collections;

public class RandomPitch : MonoBehaviour
{
    public MinMax pitch;

    void Awake()
    {
        GetComponent<AudioSource>().pitch = pitch.Random();
    }
}

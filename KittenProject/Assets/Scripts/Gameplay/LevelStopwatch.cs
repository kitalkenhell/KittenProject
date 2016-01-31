using UnityEngine;
using System.Collections;

public class LevelStopwatch : MonoBehaviour 
{
    public bool start;

    static bool stopped;
    static float startTime;

    public static float Stopwatch
    {
        get
        {
            return Time.timeSinceLevelLoad - startTime;
        }
    }

    void Start()
    {
        startTime = Time.timeSinceLevelLoad;
        stopped = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (start && stopped)
        {
            startTime = Time.timeSinceLevelLoad;
            stopped = false;
        }
        else if (!start && !stopped)
        {
            stopped = true;
        }
    }
}
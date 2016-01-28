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
            return Time.time - startTime;
        }
    }

    void Start()
    {
        startTime = Time.time;
        stopped = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (start && stopped)
        {
            startTime = Time.time;
            stopped = false;
        }
        else if (!start && !stopped)
        {
            stopped = true;
        }
    }
}
using UnityEngine;
using System.Collections;

public class LevelStopwatch : MonoBehaviour 
{
    public bool start;

    static bool stopped;
    static float startTime;
    static float endTime;

    public static float Stopwatch
    {
        get
        {
            if (startTime == Mathf.Infinity)
            {
                return 0;
            }
            else if (endTime == Mathf.Infinity)
            {
                return Time.timeSinceLevelLoad - startTime;
            }
            else
            {
                return endTime - startTime;
            }
        }
    }

    void Start()
    {
        startTime = endTime = Mathf.Infinity;
        stopped = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (start && stopped)
        {
            startTime = Time.timeSinceLevelLoad;
            PostOffice.PostLevelStopwatchStarted();
            stopped = false;
        }
        else if (!start && !stopped)
        {
            endTime = Time.timeSinceLevelLoad;
            stopped = true;
        }
    }
}
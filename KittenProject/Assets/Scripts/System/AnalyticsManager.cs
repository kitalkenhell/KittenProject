using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public static class AnalyticsManager
{
    public static void OnLevelCompleted(string sceneName, float time)
    {
        Dictionary<string, object> data = new Dictionary< string, object>
        {
            { "level", sceneName },
			{ "gameTime", time },
        };

        Analytics.CustomEvent("LevelCompleted", data);
    }

    public static void OnLevelFailed(string sceneName, float time, Vector3 position, bool killedByHellfire)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "level", sceneName },
            { "gameTime", time },
            { "position", position.ToString() },
            { "killedByHellfire", killedByHellfire.ToString() },
        };

        Analytics.CustomEvent("LevelFailed", data);
    }

    public static void OnLevelAbandoned(string sceneName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "level", sceneName },
        };

        Analytics.CustomEvent("LevelAbandoned", data);
    }

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugOverlay : MonoBehaviour 
{
    public Text textLabel;
    public bool displayUnityLogs;

	void Start () 
    {
        PostOffice.debugMessage += PrintMessage;

        if (displayUnityLogs)
        {
            Application.logMessageReceived += HandleLog;
        }
	}

	void OnDestroy () 
    {
        PostOffice.debugMessage -= PrintMessage;

        if (displayUnityLogs)
        {
            Application.logMessageReceived -= HandleLog;
        }
    }

    void PrintMessage(string message)
    {
        textLabel.text = message;
    }

    void HandleLog(string log, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            textLabel.color = Color.red;
        }
        else if (type == LogType.Warning)
        {
            textLabel.color = Color.yellow;
        }
        else
        {
            textLabel.color = Color.white;
        }

        textLabel.text = log;
    }
}

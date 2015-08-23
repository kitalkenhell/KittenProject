using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugOverlay : MonoBehaviour 
{
    public Text textLabel;

	void Start () 
    {
        PostOffice.debugMessage += PrintMessage;
	}

	void OnDestroy () 
    {
        PostOffice.debugMessage -= PrintMessage;
	}

    void PrintMessage(string message)
    {
        textLabel.text = message;
    }
}

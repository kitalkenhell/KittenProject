using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameTimeCounter : MonoBehaviour
{
    Text label;

	void Awake()
    {
        label = GetComponent<Text>();
	}

	void Update()
    {
        label.text = String.Format("{0:0.00}", LevelStopwatch.Stopwatch);
    }
}

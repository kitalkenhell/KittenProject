using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAudioSources : MonoBehaviour
{
    public AudioSource buttonClick;

    public static UiAudioSources Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

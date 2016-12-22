using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSound : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(PlaySound);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    void PlaySound()
    {
        UiAudioSources.Instance.buttonClick.Play();
    }
}

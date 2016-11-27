using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        button.onClick.AddListener(OnClicked);
    }

    void OnDisable()
    {
        button.onClick.AddListener(OnClicked);
    }

    public void OnClicked()
    {
        PostOffice.PostBackButtonClicked();
    }
}

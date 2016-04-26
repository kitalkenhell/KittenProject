using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PauseMenu : MonoBehaviour
{
    const string scaleLabel = "UI Scale: ";

    public Slider uiScaleSlider;
    public Text uiScaleText;
    public ControlsButtonsScale buttonScaler;

    void OnEnable()
    {
        uiScaleSlider.value = GameSettings.UiScale;
        uiScaleText.text = Strings.PauseMenu.uiScale + String.Format("{0:0.0}", uiScaleSlider.value);
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }
    public void OnUiScaleSlider(float scale)
    {
        GameSettings.UiScale = scale;
        uiScaleText.text = Strings.PauseMenu.uiScale + String.Format("{0:0.0}", scale);
        buttonScaler.RefreshButtons();
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    public void OnMenuButtonClicked()
    {
        PostOffice.PostLevelQuit();
        ResumeGame();
    }

    void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}

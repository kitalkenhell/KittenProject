using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class OptionsMenu : MonoBehaviour
{
    const string scaleLabel = "UI Scale: ";

    public Slider uiScaleSlider;
    public Text uiScaleText;
    public ControlsButtonsScale buttonScaler;
    public PauseMenu pauseMenu;

    Animator animator;

    int toggleAnimHash;

    void Awake()
    {
        animator = GetComponent<Animator>();

        toggleAnimHash = Animator.StringToHash("Toggle");
    }

    void OnEnable()
    {
        uiScaleSlider.value = GameSettings.UiScale;
        uiScaleText.text = Strings.PauseMenu.uiScale + String.Format("{0:0.0}", uiScaleSlider.value);

        animator.SetTrigger(toggleAnimHash);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }
    public void OnUiScaleSlider(float scale)
    {
        GameSettings.UiScale = uiScaleSlider.value;
        uiScaleText.text = Strings.PauseMenu.uiScale + String.Format("{0:0.0}", uiScaleSlider.value);
        buttonScaler.RefreshButtons();
    }

    public void OnBackButtonClicked()
    {
        GoBack();
    }

    void GoBack()
    {
        pauseMenu.BackFromOptions();
        animator.SetTrigger(toggleAnimHash);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }
}

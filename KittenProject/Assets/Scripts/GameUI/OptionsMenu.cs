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
    public Toggle musicToggle;
    public Toggle sfxToggle;
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
        uiScaleSlider.value = PersistentData.UiScale;
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
        PersistentData.UiScale = uiScaleSlider.value;
        uiScaleText.text = Strings.PauseMenu.uiScale + String.Format("{0:0.0}", uiScaleSlider.value);
        buttonScaler.RefreshButtons();
    }

    public void OnBackButtonClicked()
    {
        GoBack();
    }

    public void OnSfxToggled()
    {
        PersistentData.MusicDisabled = !sfxToggle.isOn;
        PostOffice.PostSfxToggled(sfxToggle.isOn);
    }

    public void OnMusicToggled()
    {
        PersistentData.SfxDisabled = !musicToggle.isOn;
        PostOffice.PostMusicToggled(musicToggle.isOn);
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

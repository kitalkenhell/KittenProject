using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayLevelButton : MonoBehaviour 
{
    public LevelProperties levelProperties;
    public MenuManager menu;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();

        button.interactable = !levelProperties.IsLocked;
    }

	public void OnClicked()
    {
        menu.LevelToLoad = levelProperties.sceneName;
    }
}

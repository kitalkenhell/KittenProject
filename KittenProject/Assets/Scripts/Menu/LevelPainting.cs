using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelPainting : MonoBehaviour
{
    public LevelProperties levelProperties;
    public GameObject goldenKitten;
    public GameObject goldenGem;
    public GameObject goldenHourglass;
    public GameObject greyedOutKitten;
    public GameObject greyedOutGem;
    public GameObject greyedOutHourglass;
    public AdPainting nearestAdPainting;
    public Button playButton;
    public Text levelName;
    public MenuManager menu;
    public Material blackAndWhiteMaterial;

    public SpriteRenderer[] spritesToGreyOut;

    void Start()
    {
        greyedOutGem.SetActive(!levelProperties.HasCoinStar);
        goldenGem.SetActive(levelProperties.HasCoinStar);

        greyedOutHourglass.SetActive(!levelProperties.HasTimeStar);
        goldenHourglass.SetActive(levelProperties.HasTimeStar);

        greyedOutKitten.SetActive(!levelProperties.HasGoldenKittenStar);
        goldenKitten.SetActive(levelProperties.HasGoldenKittenStar);

        playButton.interactable = !levelProperties.IsLocked;

        levelName.text = levelProperties.levelName;

        if (levelProperties.IsLocked)
        {
            foreach (var sprite in spritesToGreyOut)
            {
                sprite.material = blackAndWhiteMaterial;
            }
        }
    }

    public void OnPlayButtonClicked()
    {
        menu.LevelToLoad = levelProperties.sceneName;
        menu.OnStartGameButtonClicked();
    }
}

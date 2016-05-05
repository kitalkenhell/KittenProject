using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public LevelProperties level;
    public GameObject goldenKitten;
    public GameObject goldenGem;
    public GameObject goldenHourglass;
    public GameObject greyedOutKitten;
    public GameObject greyedOutGem;
    public GameObject greyedOutHourglass;
    public Button playButton;
    public MenuManager menu;
    public Material blackAndWhiteMaterial;

    public SpriteRenderer[] spritesToGreyOut;

    void Start()
    {
        greyedOutGem.SetActive(!level.HasCoinStar);
        goldenGem.SetActive(level.HasCoinStar);

        greyedOutHourglass.SetActive(!level.HasTimeStar);
        goldenHourglass.SetActive(level.HasTimeStar);

        greyedOutKitten.SetActive(!level.HasGoldenKittenStar);
        goldenKitten.SetActive(level.HasGoldenKittenStar);

        playButton.interactable = !level.IsLocked;

        if (level.IsLocked)
        {
            foreach (var sprite in spritesToGreyOut)
            {
                sprite.material = blackAndWhiteMaterial;
            }
        }
    }

    public void OnPlayButtonClicked()
    {
        menu.LoadLevel(level.sceneName);
    }
}

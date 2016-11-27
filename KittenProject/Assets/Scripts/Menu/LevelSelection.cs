using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
    public LevelPainting[] levels;
    public CameraPan mainCamera;
    public GameObject tutorialArrow;

    void Start()
    {
        LevelPainting levelToShow = levels.First();

        foreach (var level in levels)
        {
            if (level.levelProperties.IsLocked)
            {
                break;
            }
            else
            {
                levelToShow = level;
            }
        }

        if (levelToShow.nearestAdPainting != null && levelToShow.nearestAdPainting.SetDogIsSad())
        {
            mainCamera.Postion = levelToShow.nearestAdPainting.transform.position.x;
        }
        else
        {
            mainCamera.Postion = levelToShow.transform.position.x;
        }

        tutorialArrow.SetActive(!levels.First().levelProperties.IsCompleted);
    }
}

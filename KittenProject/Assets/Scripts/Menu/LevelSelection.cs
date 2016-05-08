using UnityEngine;
using System.Collections;

public class LevelSelection : MonoBehaviour
{
    public LevelPainting[] levels;
    public new CameraPan camera;

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
            camera.Postion = levelToShow.nearestAdPainting.transform.position.x;
        }
        else
        {
            camera.Postion = levelToShow.transform.position.x;
        }
    }
}

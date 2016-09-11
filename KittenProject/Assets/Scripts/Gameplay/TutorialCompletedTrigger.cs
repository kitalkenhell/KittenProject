using UnityEngine;
using System.Collections;

public class TutorialCompletedTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        SocialManager.UnlockAchievement(SocialManager.Achievements.finishTutorial);
    }
}

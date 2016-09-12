using UnityEngine;
using System.Collections;

public class WentAwayFromDragonTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        SocialManager.UnlockAchievement(SocialManager.Achievements.runAwayFromDragon);
    }
}

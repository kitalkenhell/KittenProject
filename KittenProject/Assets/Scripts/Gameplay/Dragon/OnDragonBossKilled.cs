using UnityEngine;
using System.Collections;

public class OnDragonBossKilled : MonoBehaviour
{
    public Animator exit;
    public GameObject[] catapultTriggers;
    public GameObject defeatedDragon;

    public void OnDragonKilled()
    {
        exit.enabled = true;
        defeatedDragon.SetActive(true);
        foreach (var trigger in catapultTriggers)
        {
            Destroy(trigger); 
        }
    }
}

using UnityEngine;
using System.Collections;

public class OnDragonBossKilled : MonoBehaviour
{
    public Animator exit;
    public GameObject catapultTriggers;

    public void OnDragonKilled()
    {
        exit.enabled = true;
        Destroy(catapultTriggers);
    }
}

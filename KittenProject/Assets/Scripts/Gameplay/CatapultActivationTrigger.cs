using UnityEngine;
using System.Collections;

public class CatapultActivationTrigger : MonoBehaviour
{
    public Catapult catapult;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        catapult.canShoot = true;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        catapult.canShoot = false;
    }
}

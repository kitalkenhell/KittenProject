using UnityEngine;
using System.Collections;

public class CatapultLever : MonoBehaviour
{
    public Catapult catapult;
    public Animator animator;

    int toggleAnimHash = Animator.StringToHash("Toggle");

    public void OnTriggerEnter2D(Collider2D collider)
    {
        catapult.canShoot = true;
        animator.SetBool(toggleAnimHash, true);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        catapult.canShoot = false;
        animator.SetBool(toggleAnimHash, false);
    }

    public void OnDestroy()
    {
        catapult.canShoot = false;

        catapult.canShoot = false;
        animator.SetBool(toggleAnimHash, false);
    }
}

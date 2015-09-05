using UnityEngine;
using System.Collections;

public class ActivationTrigger : MonoBehaviour 
{
    public bool enableOnEnter = true;
    public bool enableOnExit = false;
    public bool disableOnEnter = false;
    public bool disableOnExit = false;
    public bool disableSelfOnEnter = false;
    public bool disableSelfOnExit = false;
    public GameObject target;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (enableOnEnter)
        {
            target.SetActive(true);
        }
        else if (disableOnEnter)
        {
            target.SetActive(true);
        }

        if (disableSelfOnEnter)
        {
            gameObject.SetActive(false);
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (enableOnExit)
        {
            target.SetActive(true);
        }
        else if (disableOnExit)
        {
            target.SetActive(true);
        }

        if (disableSelfOnExit)
        {
            gameObject.SetActive(false);
        }
    }
}

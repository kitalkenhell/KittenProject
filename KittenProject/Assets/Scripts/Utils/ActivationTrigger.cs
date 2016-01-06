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
    public bool disableOnStart = true;
    public GameObject target;

    void Awake()
    {
        if (disableOnStart)
        {
            target.SetActive(false); 
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            if (enableOnEnter)
            {
                target.SetActive(true);
            }
            else if (disableOnEnter)
            {
                target.SetActive(false);
            }

            if (disableSelfOnEnter)
            {
                gameObject.SetActive(false);
            } 
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            if (enableOnExit)
            {
                target.SetActive(true);
            }
            else if (disableOnExit)
            {
                target.SetActive(false);
            }

            if (disableSelfOnExit)
            {
                gameObject.SetActive(false);
            } 
        }
    }
}

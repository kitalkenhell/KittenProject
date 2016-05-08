using UnityEngine;
using System.Collections;

public class ActivateOnEnable : MonoBehaviour
{
    public GameObject target;

    public bool activateOnEnable;
    public bool deactivateOnEnable;
    public bool activateOnDisable;
    public bool deactivateOnDisable;

    void OnEnable()
    {
        if (target != null)
        {
            if (activateOnEnable)
            {
                target.gameObject.SetActive(true);
            }
            if (deactivateOnEnable)
            {
                target.gameObject.SetActive(false);
            } 
        }
    }

    void OnDisable()
    {
        if (target != null)
        {
            if (activateOnDisable)
            {
                target.gameObject.SetActive(true);
            }
            if (deactivateOnDisable)
            {
                target.gameObject.SetActive(false);
            } 
        }
    }
}

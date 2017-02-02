using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnButtonHover : MonoBehaviour
{
    public UnityEvent onButtonDown;
    public UnityEvent onButtonUp;

    public bool isHovering
    {
        get;
        private set;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Raycast(Input.GetTouch(i).position))
            {
                return;
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Raycast(Input.mousePosition))
        {
            return;
        } 
#endif

        if (isHovering)
        {
            isHovering = false;
            onButtonUp.Invoke(); 
        }
    }

    bool Raycast(Vector3 position)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);

        pointer.position = position;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        for (int j = 0; j < raycastResults.Count; ++j)
        {
            if (raycastResults[j].gameObject == gameObject)
            {
                if (!isHovering)
                {
                    isHovering = true;
                    onButtonDown.Invoke(); 
                }
                return true;
            }
        }

        return false;
    }
}

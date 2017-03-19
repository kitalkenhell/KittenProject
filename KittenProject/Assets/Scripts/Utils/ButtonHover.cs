using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    public bool IsHovering
    {
        get;
        private set;
    }

    void Update()
    {
        IsHovering = false;

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Raycast(Input.GetTouch(i).position))
            {
                IsHovering = true;
                return;
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Raycast(Input.mousePosition))
        {
            IsHovering = true;
            return;
        } 
#endif
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
                return true;
            }
        }

        return false;
    }
}

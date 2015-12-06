using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour 
{
    public string sortingLayer = "Default";

#if UNITY_EDITOR
	void Update () 
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.sortingLayerName = sortingLayer;
        }
	}

#endif
}

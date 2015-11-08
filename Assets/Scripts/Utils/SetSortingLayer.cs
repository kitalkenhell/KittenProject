using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour 
{
    public string sortingLayer = "Default";

#if UNITY_EDITOR
	void Update () 
    {
        GetComponent<Renderer>().sortingLayerName = sortingLayer;
	}

#endif
}

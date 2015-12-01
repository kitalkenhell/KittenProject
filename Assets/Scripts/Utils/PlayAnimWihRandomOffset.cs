using UnityEngine;
using System.Collections;

public class PlayAnimWihRandomOffset : MonoBehaviour 
{
    public string stateName;
    public MinMax offset;

	void OnEnable()
    {
        GetComponent<Animator>().Play(stateName, 0,offset.Random());
	}

}

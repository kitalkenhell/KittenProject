using UnityEngine;
using System.Collections;

public class MaterialScroll : MonoBehaviour 
{
    Material material;
	
	void Start () 
    {
        material = GetComponent<MeshRenderer>().material;
	}

    void LateUpdate() 
    {
        material.mainTextureOffset = new Vector2((transform.position.x / transform.localScale.x) * material.mainTextureScale.x,
                                                 (transform.position.y / transform.localScale.y) * material.mainTextureScale.y);
	}
}

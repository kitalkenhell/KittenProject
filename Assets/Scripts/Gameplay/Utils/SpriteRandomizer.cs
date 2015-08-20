using UnityEngine;
using System.Collections;

public class SpriteRandomizer : MonoBehaviour 
{
    public Sprite[] sprites;

	void Start ()
    {
        if (sprites != null && sprites.Length > 0)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
            }
        }
	}

}

using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    public float horizontalAxis;
    public bool jumpButtonDown;

	void Start () 
    {
	    horizontalAxis = 0;
        jumpButtonDown = false;
	}
	
#if UNITY_EDITOR
	void Update ()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        jumpButtonDown = Input.GetButton("Jump") ? true : false;
	}
#endif

    public void LeftArrowDown()
    {
        horizontalAxis -= 1.0f;
    }

    public void LeftArrowUp()
    {
        horizontalAxis += 1;
    }

    public void RightArrowDown()
    {
        horizontalAxis += 1.0f;
    }

    public void RightArrowUp()
    {
        horizontalAxis -= 1.0f;
    }

    public void JumpDown()
    {
        jumpButtonDown = true;
    }

    public void JumpUp()
    {
        jumpButtonDown = false;
    }
}

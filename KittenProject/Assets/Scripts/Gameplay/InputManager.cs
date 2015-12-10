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
	
#if UNITY_EDITOR || UNITY_STANDALONE
	void Update ()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        jumpButtonDown = Input.GetButton("Jump") ? true : false;
	}
#endif

    public void LeftArrowDown()
    {
        Debug.Log("LeftArrowDown");
        horizontalAxis -= 1.0f;
    }

    public void LeftArrowUp()
    {
        Debug.Log("LeftArrowUp");
        horizontalAxis += 1;
    }

    public void RightArrowDown()
    {
        Debug.Log("RightArrowDown");
        horizontalAxis += 1.0f;
    }

    public void RightArrowUp()
    {
        Debug.Log("RightArrowUp");
        horizontalAxis -= 1.0f;
    }

    public void JumpDown()
    {
        Debug.Log("JumpDown");
        jumpButtonDown = true;
    }

    public void JumpUp()
    {
        Debug.Log("JumpUp");
        jumpButtonDown = false;
    }
}

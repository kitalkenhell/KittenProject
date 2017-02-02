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
        //horizontalAxis = Input.GetAxis("Horizontal");
        jumpButtonDown = Input.GetButton("Jump") ? true : false;
    }
#endif

    public void Reset()
    {
        horizontalAxis = 0;
        jumpButtonDown = false;
    }

    public void LeftArrowDown()
    {
        horizontalAxis -= 1.0f;
        ClampHorizontalAxis();
    }

    public void LeftArrowUp()
    {
        horizontalAxis += 1;
        ClampHorizontalAxis();
    }

    public void RightArrowDown()
    {
        horizontalAxis += 1.0f;
        ClampHorizontalAxis();
    }

    public void RightArrowUp()
    {
        horizontalAxis -= 1.0f;
        ClampHorizontalAxis();
    }

    public void JumpDown()
    {
        jumpButtonDown = true;
    }

    public void JumpUp()
    {
        jumpButtonDown = false;
    }

    void ClampHorizontalAxis()
    {
        horizontalAxis = Mathf.Clamp(horizontalAxis, -1, 1);
    }
}

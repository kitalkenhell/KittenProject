using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    public float horizontalAxis;
    public bool jumpButtonDown;

    public ButtonHover leftButton;
    public ButtonHover rightButton;

    void Start () 
    {
	    horizontalAxis = 0;
        jumpButtonDown = false;
	}
	
	void Update ()
    {
#if UNITY_EDITOR || UNITY_STANDALONE

        horizontalAxis = Input.GetAxis("Horizontal");
        jumpButtonDown = Input.GetButton("Jump") ? true : false;
#else
        horizontalAxis = 0;

        if (!leftButton.IsHovering || !rightButton.IsHovering)
        { 
            if (leftButton.IsHovering)
            {
                horizontalAxis = -1;
            }
            else if (rightButton.IsHovering)
            {
                horizontalAxis = 1;
            }
        }
#endif
    }

    public void Reset()
    {
        horizontalAxis = 0;
        jumpButtonDown = false;
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

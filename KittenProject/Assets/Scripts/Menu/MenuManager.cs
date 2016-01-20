using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
    int playButtonPressedAnimHash;
    int backButtonPressedAnimHash;

    Animator animator;

	void Start () 
	{
        playButtonPressedAnimHash = Animator.StringToHash("PlayButtonPressed");
        backButtonPressedAnimHash = Animator.StringToHash("BackButtonPressed");

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            animator.SetTrigger(backButtonPressedAnimHash);
        }
    }

    public void OnPlayButtonClicked()
    {
        animator.SetTrigger(playButtonPressedAnimHash);
    }
}

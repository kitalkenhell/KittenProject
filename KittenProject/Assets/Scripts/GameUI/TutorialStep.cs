using UnityEngine;
using System.Collections;

public class TutorialStep : MonoBehaviour
{
    public Animator tutorialUi;

    int stepAnimHash = Animator.StringToHash("Step");
    
    void OnTriggerEnter2D(Collider2D other)
    {
        tutorialUi.SetTrigger(stepAnimHash);
        Destroy(gameObject);    
    }
}

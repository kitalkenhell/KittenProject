using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CutsceneController : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneAction
    {
        public float delay;
        public UnityEvent action;
    }

    public CutsceneAction[] actions;

    void Start()
    {
        PlayCutscene();
    }

    void PlayCutscene()
    {
        StartCoroutine(ExecuteActions());
    }

    IEnumerator ExecuteActions()
    {
        foreach (var action in actions)
        {
            yield return new WaitForSeconds(action.delay);
            action.action.Invoke();
        }
    }
}

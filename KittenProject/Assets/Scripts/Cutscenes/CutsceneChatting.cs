using UnityEngine;
using System.Collections;

public class CutsceneChatting : MonoBehaviour
{
    public SpeechBalloon[] speechBalloons;

    bool skip = false;

    public bool InProgress
    {
        private set;
        get;
    }

    public bool HasEnded
    {
        private set;
        get;
    }

    void Awake()
    {
        InProgress = HasEnded = false;
    }

    public void Chat()
    {
        if (!InProgress)
        {
            skip = false;
            HasEnded = false;
            InProgress = true;
            StartCoroutine(Chatting());
        }
    }

    public void Skip()
    {
        skip = true;
    }

    IEnumerator Chatting()
    {
        if (speechBalloons.Length > 0)
        {
            speechBalloons.First().Show();
        }

        for (int i = 0; i < speechBalloons.Length; ++i)
        {
            while (true)
            {
                if (Clicked())
                {
                    speechBalloons[i].Show(false);

                    if (i + 1 < speechBalloons.Length)
                    {
                        speechBalloons[i + 1].Show();
                    }

                    yield return null;
                    break;
                }
                else if (skip)
                {
                    speechBalloons[i].Show(false);
                    HasEnded = true;
                    StopAllCoroutines();
                    yield return null;
                }

                yield return null; 
            }
        }

        HasEnded = true;
    }

    bool Clicked()
    {
        const int leftClick = 0;

        if (Input.GetMouseButtonUp(leftClick))
        {
            return true;
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                return true;
            }
        }
        return false;
    }

}

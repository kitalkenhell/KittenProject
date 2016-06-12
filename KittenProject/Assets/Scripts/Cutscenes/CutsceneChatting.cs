using UnityEngine;
using System.Collections;

public class CutsceneChatting : MonoBehaviour
{
    public SpeechBalloon[] speechBalloons;

    bool inProgress;

    public void Chat()
    {
        if (!inProgress)
        {
            StartCoroutine(Chatting());
        }
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

                yield return null; 
            }
        }
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

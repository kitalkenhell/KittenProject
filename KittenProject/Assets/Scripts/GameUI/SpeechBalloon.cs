using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class SpeechBalloon : MonoBehaviour
{
    [TextArea] public string text;
    public Text textLabel;
    public float delayPerLetter;

    Animator animator;
    
    int showAnimHash;

    StringBuilder builder;

    void Awake()
    {
        animator = GetComponent<Animator>();

        showAnimHash = Animator.StringToHash("Show");

        builder = new StringBuilder(text.Length);

        if (delayPerLetter > 0)
        {
            textLabel.text = SetEmptyText();
        }
        else
        {
            textLabel.text = text;
        }
    }

    string SetEmptyText()
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '\n')
            {
                builder.Append(" ");
            }
            else
            {
                builder.Append(text[i]);
            }
        }

        return builder.ToString();
    }

    IEnumerator ShowLetters()
    {
        int i = 0;

        while (i < text.Length)
        {
            if (i < textLabel.text.Length)
            {
                builder[i] = text[i];
                textLabel.text = builder.ToString();
                ++i;
                yield return new WaitForSeconds(delayPerLetter);
            }
            else
            {
                break;
            }
        }
    }

    public void Show(bool show = true)
    {
        animator.SetBool(showAnimHash, show);

        if (delayPerLetter > 0)
        {
            StartCoroutine(ShowLetters());
        }
    }
}

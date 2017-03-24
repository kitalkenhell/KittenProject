using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsWidget : MonoBehaviour
{
    public Text amountLabel;
    public float changingSmoothTime;

    float currentAmount;
    float changingSpeed;
    bool isAnimating;

    void OnEnable()
    {
        isAnimating = false;
    }

    void Start()
    {
        PostOffice.amountOfCoinsChanged += OnAmountOfCoinsChanged;

        currentAmount = PersistentData.Coins;
        amountLabel.text = PersistentData.Coins.ToString();
    }

    void OnDestroy()
    {
        PostOffice.amountOfCoinsChanged -= OnAmountOfCoinsChanged;
    }

    void OnAmountOfCoinsChanged()
    {
        if (gameObject.activeInHierarchy && enabled)
        {
            if (!isAnimating)
            {
                isAnimating = true;
                StartCoroutine(Animate());
            }
        }
        else
        {
            amountLabel.text = PersistentData.Coins.ToString();
        }
    }

    IEnumerator Animate()
    {
        const float threshold = 1;

        changingSpeed = 0;

        while (Mathf.Abs(currentAmount - PersistentData.Coins) > threshold)
        {
            currentAmount = Mathf.SmoothDamp(currentAmount, PersistentData.Coins, ref changingSpeed, changingSmoothTime);
            amountLabel.text = ((int)currentAmount).ToString();
            yield return null;
        }

        currentAmount = PersistentData.Coins;
        amountLabel.text = PersistentData.Coins.ToString();
        isAnimating = false;
    }
}

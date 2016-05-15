using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsWidget : MonoBehaviour
{
    public Text amountLabel;

    void Start()
    {
        OnAmountOfCoinsChanged();

        PostOffice.amountOfCoinsChanged += OnAmountOfCoinsChanged;
    }

    void OnDestroy()
    {
        PostOffice.amountOfCoinsChanged -= OnAmountOfCoinsChanged;
    }

    void OnAmountOfCoinsChanged()
    {
        amountLabel.text = PersistentData.Coins.ToString();
    }
}

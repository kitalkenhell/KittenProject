using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsCounter : MonoBehaviour 
{
    int count;

    Text label;

    void Start()
    {
        label = GetComponent<Text>();

        PostOffice.coinCollected += OnCoinCollected;
        PostOffice.coinDropped += OnCoinDropped;
    }

    void OnDestroy()
    {
        PostOffice.coinCollected -= OnCoinCollected;
        PostOffice.coinDropped -= OnCoinDropped;
    }

    void OnCoinCollected(int amount)
    {
        count += amount;
        label.text = count.ToString();
    }

    void OnCoinDropped(int amount)
    {
        count -= amount;
        label.text = count.ToString();
    }

}

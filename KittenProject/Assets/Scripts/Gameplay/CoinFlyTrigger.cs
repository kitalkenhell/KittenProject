using UnityEngine;
using System.Collections;

public class CoinFlyTrigger : MonoBehaviour 
{
    public AudioSource pickUpSound;
    public int amountOfCoins;

    CoinMover coinMover;
    MoveAlongCurve curveMover;
    bool collected;

    void Start()
    {
        coinMover = GetComponent<CoinMover>();
        curveMover = GetComponent<MoveAlongCurve>();

        coinMover.enabled = false;
        collected = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!collected)
        {
            collected = true;
            enabled = false;
            coinMover.enabled = true;
            pickUpSound.Play();
            pickUpSound.transform.parent = null;
            curveMover.enabled = false;

            GetComponent<CoinEmitter>().BurstEmit(amountOfCoins);
        }
    }
}

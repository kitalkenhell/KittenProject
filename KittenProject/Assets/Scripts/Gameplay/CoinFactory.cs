using UnityEngine;
using System.Collections;
using System;

public class CoinFactory : MonoBehaviour 
{
    public int coinsCount;
    public GameObject coinPrefab;
    public PlayerLogic player;

    DynamicCoinResetter[] coins;
    int currentCoin;

	void Start() 
    {
        currentCoin = 0;
        coins = new DynamicCoinResetter[coinsCount];

        for (int i = 0; i < coins.Length; i++)
        {
            coins[i] = Instantiate(coinPrefab).GetComponent<DynamicCoinResetter>();
            coins[i].gameObject.SetActive(false);
        }
	}

	public void Spawn(Vector3 position, Vector2 velocity, float angularSpeed)
    {
        coins[currentCoin].Reset(position, velocity, angularSpeed, player);

        if (++currentCoin >= coins.Length)
        {
            currentCoin = 0;
        }
    }
}

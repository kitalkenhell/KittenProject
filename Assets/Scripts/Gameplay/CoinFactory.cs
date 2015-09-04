using UnityEngine;
using System.Collections;

public class CoinFactory : MonoBehaviour 
{
    public int coinsCount;
    public GameObject coinPrefab;

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

	public void Spawn(Vector3 position, Vector2 velocity, float angularSpeed, Transform target)
    {
        coins[currentCoin].Reset(position, velocity,angularSpeed, target);

        if (++currentCoin >= coins.Length)
        {
            currentCoin = 0;
        }
    }
}

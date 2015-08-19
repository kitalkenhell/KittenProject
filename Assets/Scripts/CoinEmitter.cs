using UnityEngine;
using System.Collections;

public class CoinEmitter : MonoBehaviour
{
    public GameObject coinPrefab;
    public float emitTickInterval = 0.02f;
    public float coinsPerTick = 3;
    public float VerticalForceMin;
    public float VerticalForceMax;
    public float HorizontalForceMin;
    public float HorizontalForceMax;

    void Emit(int amount)
    {
        StartCoroutine(EmitCourutine(amount));
    }
    
    IEnumerator EmitCourutine(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            GameObject coin = Instantiate(coinPrefab);
            Rigidbody2D coinBody = coin.GetComponent<Rigidbody2D>();
            coin.GetComponent<CoinMover>().target = transform;

            coin.transform.position = transform.position;
            coinBody.velocity = new Vector2(Utils.RandomSign() * Random.Range(HorizontalForceMin, HorizontalForceMax), Random.Range(VerticalForceMin, VerticalForceMax));

            if (i % coinsPerTick == 0)
            {
                yield return new WaitForSeconds(emitTickInterval);
            }
        }
        
    }
}

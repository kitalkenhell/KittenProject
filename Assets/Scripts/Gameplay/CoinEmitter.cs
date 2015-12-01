using UnityEngine;
using System.Collections;

public class CoinEmitter : MonoBehaviour
{
    public CoinFactory coinFactory;
    public float emitTickInterval = 0.02f;
    public float coinsPerTick = 3;
    public MinMax VerticalForce;
    public MinMax HorizontalForce;
	public MinMax angularSpeed;

    public void Emit(int amount)
    {
        StartCoroutine(EmitCourutine(amount));
    }
    
    IEnumerator EmitCourutine(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            Vector2 velocity = new Vector2(Utils.RandomSign() * HorizontalForce.Random(), VerticalForce.Random());

            coinFactory.Spawn(transform.position, velocity, angularSpeed.Random() * Utils.RandomSign());
            PostOffice.PostCoinDropped();

            if (i % coinsPerTick == 0)
            {
                yield return new WaitForSeconds(emitTickInterval);
            }
        }
    }
}

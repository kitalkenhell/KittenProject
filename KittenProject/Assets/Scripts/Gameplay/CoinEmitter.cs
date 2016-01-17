using UnityEngine;
using System.Collections;

public class CoinEmitter : MonoBehaviour
{
    public CoinFactory coinFactory;
    public float emitTickInterval = 0.02f;
    public float coinsPerTick = 3;
    public MinMax verticalForce;
    public MinMax horizontalForce;
    public bool horizontalForceRandomSign;
    public bool verticalForceRandomSign;
    public MinMax angularSpeed;
    public float emitterRadius;
    public float gravityNegationFactor = 1;

    public void Emit(int amount)
    {
        StartCoroutine(EmitCourutine(amount));
    }

    public void BurstEmit(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            EmitCoin();
        }
    }

    void EmitCoin()
    {
        Vector2 velocity = new Vector2(horizontalForce.Random(), verticalForce.Random());

        if (horizontalForceRandomSign)
        {
            velocity.x *= Utils.RandomSign();
        }

        if (verticalForceRandomSign)
        {
            velocity.y *= Utils.RandomSign();
        }

        if (Mathf.Sign(velocity.y) == Mathf.Sign(Physics2D.gravity.y))
        {
            velocity.y *= gravityNegationFactor;
        }

        coinFactory.Spawn(transform.position + Random.insideUnitCircle.Vec3() * emitterRadius, velocity, angularSpeed.Random() * Utils.RandomSign());
    }
    
    IEnumerator EmitCourutine(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            EmitCoin();
            if (i % coinsPerTick == 0)
            {
                yield return new WaitForSeconds(emitTickInterval);
            }
        }
    }
}


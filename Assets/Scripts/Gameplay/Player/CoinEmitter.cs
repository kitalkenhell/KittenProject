﻿using UnityEngine;
using System.Collections;

public class CoinEmitter : MonoBehaviour
{
    public CoinFactory coinFactory;
    public float emitTickInterval = 0.02f;
    public float coinsPerTick = 3;
    public float VerticalForceMin;
    public float VerticalForceMax;
    public float HorizontalForceMin;
    public float HorizontalForceMax;

    //void Start()
    //{
    //    InvokeRepeating("Test", 3.0f, 5.0f);
    //}

    //void Test()
    //{
    //    StartCoroutine(EmitCourutine(20));
    //}

    void Emit(int amount)
    {
        StartCoroutine(EmitCourutine(amount));
    }
    
    IEnumerator EmitCourutine(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            Vector2 velocity = new Vector2(Utils.RandomSign() * Random.Range(HorizontalForceMin, HorizontalForceMax), Random.Range(VerticalForceMin, VerticalForceMax));

            coinFactory.Spawn(transform.position, velocity, transform);

            if (i % coinsPerTick == 0)
            {
                yield return new WaitForSeconds(emitTickInterval);
            }
        }
    }
}
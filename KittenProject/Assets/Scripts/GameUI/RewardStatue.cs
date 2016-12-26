using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardStatue : MonoBehaviour
{
    public AudioSource impactSource;
    public AudioSource whooshSource;

    public void PlayImpactSound()
    {
        impactSource.Play();
    }

    public void PlayWhooshSound()
    {
        whooshSource.Play();
    }
}

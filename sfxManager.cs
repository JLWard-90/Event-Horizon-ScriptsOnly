using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    [SerializeField]
    AudioSource shieldEffect;
    [SerializeField]
    AudioSource fuelEffect;
    [SerializeField]
    AudioSource explosion_1;
    [SerializeField]
    AudioSource explosion_2;
    public void PlayShieldSound()
    {
        shieldEffect.Play();
    }
    public void PlayFuelSound()
    {
        fuelEffect.Play();
    }
    public void PlayExplosion1()
    {
        explosion_1.Play();
    }
    public void PlayExplosion2()
    {
        explosion_2.Play();
    }
}

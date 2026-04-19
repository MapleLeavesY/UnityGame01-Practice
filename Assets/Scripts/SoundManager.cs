using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip fuelPickUpAudioClip;
    [SerializeField] private AudioClip coinPickUpAudioClip;
    private void Start()
    {
        Lander.Instance.CoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.FuelPickUp += Lander_OnFuelPickUp;
    }

    private void Lander_OnCoinPickUp(object sender, System.EventArgs e)
    {
        //AudioSource.PlayClipAtPoint(fuelPickUpAudioClip());
    }
    private void Lander_OnFuelPickUp()
    {
        
    }
}   

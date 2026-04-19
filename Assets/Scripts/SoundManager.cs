using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private const int SOUNDVOLUMEMAX = 10;
    public static SoundManager Instance
    {
        private set;
        get;
    }
    private int gameSoundVolume = 5;
    //private int backgroundSoundVolume = 0;
    [SerializeField] private AudioClip fuelPickUpAudioClip;
    [SerializeField] private AudioClip coinPickUpAudioClip;
    [SerializeField] private AudioClip OnUnLanded;
    [SerializeField] private AudioClip OnLanded;

    public event Action OnSoundVolumeChanged;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.CoinPickUp += Lander_OnCoinPickUp;
        Lander.Instance.FuelPickUp += Lander_OnFuelPickUp;
        Lander.Instance.EmergencyLanding += Lander_OnEmergencyLanding;
        Lander.Instance.OnLandingType += Lander_OnLandingType;
    }
    private void Lander_OnCoinPickUp(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }
    private void Lander_OnEmergencyLanding(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(OnUnLanded, Camera.main.transform.position, GetSoundVolumeNormalized());
    }
    private void Lander_OnLandingType()
    {
        AudioSource.PlayClipAtPoint(OnLanded, Camera.main.transform.position, GetSoundVolumeNormalized());
    }
    private void Lander_OnFuelPickUp()
    {
        AudioSource.PlayClipAtPoint(fuelPickUpAudioClip, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    public void ChangeSoundVolume()
    {
        gameSoundVolume = (gameSoundVolume + 1) % SOUNDVOLUMEMAX;
        OnSoundVolumeChanged?.Invoke();
    }
    public int GetSoundVolume()
    {
        return gameSoundVolume;
    }
    public float GetSoundVolumeNormalized()
    {
        return ((float)gameSoundVolume) / SOUNDVOLUMEMAX;
    }
}   

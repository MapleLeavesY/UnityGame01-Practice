using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance
    {
        private set;
        get;
    }

    private const int MUSICVOLUMEMAX = 10;
    private static int musicVolume = 6;
    private static float musicTime;
    private AudioSource musicAudioSource;
    private event Action OnMusicVolumeChanged;
    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }
    private void Start()
    {
        musicAudioSource.volume = GetMusicVolumeNormalized();
    }
    private void Update()
    {
        musicTime = musicAudioSource.time;
    }
    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSICVOLUMEMAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke();
    }
    public int GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSICVOLUMEMAX;
    }
}

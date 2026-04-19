using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;
    Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }
    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnUpForce += Lander_OnUpForce;
        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChangerd;
        thrusterAudioSource.Pause();
    }
    private void SoundManager_OnSoundVolumeChangerd()
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }
    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        thrusterAudioSource.Pause();
    }
    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying)
            thrusterAudioSource.Play();
    }
    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying)
            thrusterAudioSource.Play();
    }
    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        if(!thrusterAudioSource.isPlaying)
            thrusterAudioSource.Play();
    }
}    
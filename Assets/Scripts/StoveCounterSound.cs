using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float volume = 1f;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        SoundManager.Instance.OnSoundChanged += SoundManager_OnSoundChanged;
    }

    private void SoundManager_OnSoundChanged(object sender, SoundManager.OnSoundChangedEventArgs e) {
        audioSource.volume = e.volumeArg;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if (playSound)
        {
            audioSource.Play();
        } 
        else
        {
            audioSource.Pause();
        }
    }
}

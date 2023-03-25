using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMenager : MonoBehaviour
{
    public static MusicMenager Instance { get; private set; }
    private float volume = 1f;
    private AudioSource AudioSource;

    private void Start() {
        Instance = this;

        AudioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolume() {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        AudioSource.volume = volume;
    }

    public float GetVolume() {
        return volume;
    }
}

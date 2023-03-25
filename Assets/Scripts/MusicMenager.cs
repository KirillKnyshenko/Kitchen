using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMenager : MonoBehaviour
{
    public static MusicMenager Instance { get; private set; }

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    [SerializeField] private float volume = .3f;
    private AudioSource AudioSource;

    private void Start() {
        Instance = this;

        AudioSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);

        AudioSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        
        if (volume >= 1.1f)
        {
            volume = 0f;
        }

        AudioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }
}

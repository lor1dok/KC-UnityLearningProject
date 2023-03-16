using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour {

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";


    public static MusicManager Instance {
        get; 
        private set;
    }

    private float volume = 1f;

    private AudioSource musicSource;

    private void Awake() {
        Instance = this;
        musicSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 1f);
        musicSource.volume = volume;
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1.01f) {
            volume = 0f;
        }
        musicSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() {
        return volume;
    }

}
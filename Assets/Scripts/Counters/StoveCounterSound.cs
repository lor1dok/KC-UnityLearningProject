using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoveCounterSound : MonoBehaviour {

    public static StoveCounterSound Instance {
        get; 
        private set;
    }

    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private float volume = 1f;

    private void Awake() {

        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;

        volume = PlayerPrefs.GetFloat(SoundManager.Instance.GetPlayerPrefsSound(), 1f);
        audioSource.volume = volume;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        if(e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried) {
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }

    public void ChangeVolume() {
        volume += .1f;
        if (volume > 1.01f) {
            volume = 0f;
        };

        audioSource.volume = volume;
    }
}

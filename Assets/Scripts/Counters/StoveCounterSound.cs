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
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake() {

        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;

        volume = PlayerPrefs.GetFloat(SoundManager.Instance.GetPlayerPrefsSound(), 1f);
        audioSource.volume = volume;
    }

    private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnShowProgressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        if(e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried) {
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }

    private void Update() {
        if (playWarningSound) {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer < 0) {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
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

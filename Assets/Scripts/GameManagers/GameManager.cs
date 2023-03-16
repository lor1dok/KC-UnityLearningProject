using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { 
        get; 
        private set; 
    }

    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    private enum State {
        WaitingForStart,
        CountDownForStart,
        GamePlaying,
        GameOver,
    }
    private State state;

    private float waitingForStartTimer = 2f;
    private float countDownForStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 60f;

    private bool IsGamePaused;

    private void Awake() {
        Instance = this;

        state = State.WaitingForStart;
        gamePlayingTimer = gamePlayingTimerMax;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        ToggleGamePause();
    }

    private void Update() {
        //Game mode state machine
        switch (state) {
            case State.WaitingForStart:
                waitingForStartTimer -= Time.deltaTime;
                if(waitingForStartTimer <= 0 ) {
                    state = State.CountDownForStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break; 
            case State.CountDownForStart:
                countDownForStartTimer -= Time.deltaTime;
                if(countDownForStartTimer <= 0 ) { 
                    state = State.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0 ) { 
                    state = State.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountingDownForStart() {
        return state == State.CountDownForStart;
    }

    public float GetCountDownToStartTimer() {
        return countDownForStartTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized() {
        return gamePlayingTimer / gamePlayingTimerMax; 
    }

    public void ToggleGamePause() {
        if (!IsGamePaused) {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            IsGamePaused = !IsGamePaused;
            Time.timeScale = 0f;
        }
        else {
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            IsGamePaused = !IsGamePaused;
            Time.timeScale = 1f;
        }
    }

}

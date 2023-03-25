using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnPaused;
    public event EventHandler OnUnpaused;
    public event EventHandler OnStateChanged;
    public enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        Gameover,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 15f;
    private bool isGamePaused = false;
    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void Update() {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;

                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;

                    gamePlayingTimer = gamePlayingTimerMax;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0f)
                {
                    state = State.Gameover;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Gameover:
                OnStateChanged?.Invoke(this, EventArgs.Empty);
                break;
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer() {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized() {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public bool IsGameOver() {
        return state == State.Gameover;
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnPaused?.Invoke(this, EventArgs.Empty);
        } 
        else
        {
            Time.timeScale = 1f;
            OnUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}

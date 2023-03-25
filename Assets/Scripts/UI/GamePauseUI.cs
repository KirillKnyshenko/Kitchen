using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    
    private void Start() {
        GameManager.Instance.OnPaused += GameManager_OnPaused;
        GameManager.Instance.OnUnpaused += GameManager_OnUnpaused;

        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            OptionsUI.Instance.Show(Show);

            Hide();
        });

        Hide();
    }

    private void GameManager_OnPaused(object sender, EventArgs e) {
        Show();
    }

    private void GameManager_OnUnpaused(object sender, EventArgs e) {
        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

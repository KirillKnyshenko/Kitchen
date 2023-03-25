using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance;

    [SerializeField] private Button soundsEffectButton;
    [SerializeField] private TextMeshProUGUI soundsEffectText;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Button closeButton;

    private void Start() {
        Instance = this;

        GameManager.Instance.OnUnpaused += GameManager_OnUnpaused;

        Hide();
    }

    private void Awake() {
        soundsEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();

            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicMenager.Instance.ChangeVolume();

            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    private void GameManager_OnUnpaused(object sender, EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        soundsEffectText.text = "Sounds effect: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicMenager.Instance.GetVolume() * 10f);
    }

    public void Show() {
        gameObject.SetActive(true);

        UpdateVisual();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}

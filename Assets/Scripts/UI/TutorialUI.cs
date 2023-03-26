using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAltText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebin;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        UpdateVisual();
    }

    private void UpdateVisual() {
        keyMoveUpText.text = GameInput.Instance.GetBinding(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBinding(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBinding(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBinding(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBinding(GameInput.Binding.Interactive);
        keyInteractAltText.text = GameInput.Instance.GetBinding(GameInput.Binding.InteractiveAlternate);
        keyPauseText.text = GameInput.Instance.GetBinding(GameInput.Binding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetBinding(GameInput.Binding.Gamepad_Interactive);
        keyGamepadInteractAltText.text = GameInput.Instance.GetBinding(GameInput.Binding.Gamepad_InteractiveAlternate);
        keyGamepadPauseText.text = GameInput.Instance.GetBinding(GameInput.Binding.Gamepad_Pause);
    }

    private void GameInput_OnBindingRebin(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

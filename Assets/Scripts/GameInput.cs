using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlterAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    private PlayerInputActions playerInputActions;

    public enum Binding {
        Interactive,
        InteractiveAlternate,
        Pause,
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Gamepad_Interactive,
        Gamepad_InteractiveAlternate,
        Gamepad_Pause,
    }

    private void Awake() {
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlter.performed += InteractAlter_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Start() {
        Instance = this;
    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlter.performed -= InteractAlter_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }

    private void InteractAlter_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlterAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetMoveVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBinding(Binding binding) {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interactive:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractiveAlternate:
                return playerInputActions.Player.InteractAlter.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interactive:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractiveAlternate:
                return playerInputActions.Player.InteractAlter.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
            break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
            break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
            break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
            break;
            case Binding.Interactive:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
            break;
            case Binding.InteractiveAlternate:
                inputAction = playerInputActions.Player.InteractAlter;
                bindingIndex = 0;
            break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
            break;
            case Binding.Gamepad_Interactive:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
            break;
            case Binding.Gamepad_InteractiveAlternate:
                inputAction = playerInputActions.Player.InteractAlter;
                bindingIndex = 1;
            break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
            break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete( callback => {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();
                
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}

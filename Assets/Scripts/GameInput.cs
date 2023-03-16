using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance {
        get;
        private set;
    }

    public event EventHandler OnInteractAction;
    public event EventHandler OnAlternativeInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;




    public enum Binding {
        Move_Up,
        Move_Down, 
        Move_Left, 
        Move_Right,
        Interact,
        AlternativeInteract,
        Pause,
    }

    private GameInputActions playerInputAction;

    private void Awake() {

        Instance = this;

        playerInputAction = new GameInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.AlternativeInteract.performed += AlternativeInteract_performed; ;
        playerInputAction.Player.Pause.performed += Pause_performed;


    }

    private void OnDestroy() {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.AlternativeInteract.performed -= AlternativeInteract_performed; ;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void AlternativeInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAlternativeInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetNormalizedMovementVector() {

        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public string GetBindingText(Binding binding) {
        switch(binding) {
            default:
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.AlternativeInteract:
                return playerInputAction.Player.AlternativeInteract.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebind) {
        playerInputAction.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.AlternativeInteract:
                inputAction = playerInputAction.Player.AlternativeInteract;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;


        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
            playerInputAction.Enable();
            onActionRebind();
            
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();

            callback.Dispose();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();



    }
}

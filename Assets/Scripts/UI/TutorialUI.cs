using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadMoveText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;


    private void Start() {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;

        UpdateVisual();
        Show();
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {

        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAlternativeText.text = GameInput.Instance
            .GetBindingText(GameInput.Binding.AlternativeInteract);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        keyGamepadMoveText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyGamepadInteractAlternativeText.text = GameInput.Instance
            .GetBindingText(GameInput.Binding.AlternativeInteract);
        keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractableObject : MonoBehaviour {
    
    protected abstract void OnInteract();
    
    private void OnMouseOver() {
        Debug.Log("Mouse over");
        if (Mouse.current.rightButton.wasPressedThisFrame) {
            Debug.LogWarning("Clicked!");
            OnInteract();
        }
    }
    
    private void OnEnable() {
        Debug.LogWarning("Enabling!");
        GameManager.Instance.GetPlayerInput().onActionTriggered += InteractInput;
    }

    private void OnDisable() {
        GameManager.Instance.GetPlayerInput().onActionTriggered -= InteractInput;
    }

    private void InteractInput(InputAction.CallbackContext context) {
        if (context.action.name == "Interact" && context.ReadValueAsButton()) {
            Debug.Log("Interact!");
            OnInteract();
        }
    }
}

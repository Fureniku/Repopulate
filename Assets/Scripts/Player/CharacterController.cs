using UnityEngine;
using UnityEngine.InputSystem;

//Wrapper to control the currently selected droid from the player, plus anything global for the player
public class CharacterController : MonoBehaviour {

    [Header("Gameplay stuff")]
    [SerializeField] private DroidController currentDroid;

    private bool isPlayerDroidActive = true; //True when a droid is currently controllable, false when in a UI or another mode

    public DroidController GetCurrentDroid() => currentDroid;

    public void SetDroid(DroidController newDroid) {
        currentDroid.SetCameraStatus(false);
        currentDroid = newDroid;
        currentDroid.SetCameraStatus(true);
        transform.SetParent(newDroid.transform, false);
    }
    
    public void HandleSwitch(InputAction.CallbackContext context) {
        SetActive(false);
        GameManager.Instance.SwitchControlType(EnumControlType.SHIP);
    }

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        currentDroid.SetDroidActive(true);
    }

    public void SetPlayerActive(bool active) {
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        isPlayerDroidActive = active;
        currentDroid.SetDroidActive(true);
    }

    public void SetActive(bool active) {
        Debug.Log($"Droid active: {active}");
        currentDroid.SetDroidActive(active);
    }

    public void HandleOpenInventory(InputAction.CallbackContext context) {
        if (context.performed) {
            isPlayerDroidActive = !isPlayerDroidActive;
            Cursor.lockState = isPlayerDroidActive ? CursorLockMode.Locked : CursorLockMode.None;
            currentDroid.InventoryVisible(!isPlayerDroidActive);
        }
    }

    public void HandleActiveToggle(InputAction.CallbackContext context) {
        SetPlayerActive(!isPlayerDroidActive);
    }

    public void HandleMovement(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandleMovement(context.ReadValue<Vector2>());
        }
    }

    public void HandleSwitchDroid(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            GameManager.Instance.GetDroidManager.AssignNextAvailableDroid(this);
        }
    }

    public void HandleObjectRotation(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandleObjectRotation();
        }
    }

    public void HandleJump(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandleJump();
        }
    }

    public void HandleStabilisation(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandleStabilisation();
        }
    }
    
    public void HandleCamera(InputAction.CallbackContext context) {
        if (isPlayerDroidActive && Cursor.lockState == CursorLockMode.Locked) {
            currentDroid.HandleCamera(context);
        }
    }

    public void HandleInteract(InputAction.CallbackContext context) {
        if (context.performed && isPlayerDroidActive && Cursor.lockState == CursorLockMode.Locked) {
            currentDroid.Interact();
        }
    }

    public void ResetCamera() {
        currentDroid.ResetCamera();
    }

    public void HandleUIInput(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandleUIControlInput(context);
        }
    }
    
    public void HandlePlaceObject(InputAction.CallbackContext context) {
        if (isPlayerDroidActive) {
            currentDroid.HandlePlaceObject(context);
        }
        
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager> {

    [SerializeField] private Camera fpCam;
    [SerializeField] private Camera shipCam;
    [SerializeField] private CharacterController character;
    [SerializeField] private ShipMoveController shipMoveController;
    
    public void SwitchCamera() { 
        SetCameraState(!fpCam.gameObject.activeSelf);
        SetShipControlsActive(shipCam.gameObject.activeSelf);
    }

    private void SetCameraState(bool fpActive) {
        fpCam.gameObject.SetActive(fpActive);
        shipCam.gameObject.SetActive(!fpActive);
        character.SetPlayerActive(fpActive);
    }

    private void SetShipControlsActive(bool active) {
        shipMoveController.SetActive(active);
    }

    public PlayerInput GetPlayerInput() {
        return character.GetComponent<PlayerInput>();
    }
    
    public ShipMoveController GetShipController() => shipMoveController;
}

using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager> {

    [SerializeField] private Camera fpCam;
    [SerializeField] private Camera shipCam;
    [SerializeField] private CharacterController character;
    [SerializeField] private ShipMoveController shipMoveController;
    [SerializeField] private float solarGridScale = 10000f;
    
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

    public void SwitchControlType(EnumControlType controlType) {
        switch (controlType) {
            case EnumControlType.DROID:
                Debug.Log("Switching to droid");
                GetPlayerInput().SwitchCurrentActionMap("General");
                character.SetActive(true);
                break;
            case EnumControlType.SHIP:
                GetPlayerInput().SwitchCurrentActionMap("Ship");
                shipMoveController.SetActive(true);
                break;
        }
    }

    public PlayerInput GetPlayerInput() {
        return character.GetComponent<PlayerInput>();
    }
    
    public ShipMoveController GetShipController() => shipMoveController;

    public float GetSolarGridScale() => solarGridScale;
}

public enum EnumControlType {
    DROID,
    SHIP
}

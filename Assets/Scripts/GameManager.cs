using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager> {

    [SerializeField] private Camera shipCam;
    [SerializeField] private CharacterController character;
    [SerializeField] private ShipMoveController shipMoveController;
    [SerializeField] private SolarSystemManager solarSystem;
    
    [SerializeField] private Item emptyItem;

    private DroidManager _droidManager;

    //An empty item with no logic, model or assets, used instead of null for missing items or unoccupied scrollbar slots
    public Item EmptyItem => emptyItem;
    public DroidManager GetDroidManager => _droidManager;

    void Start() {
        _droidManager = GetComponent<DroidManager>();
    }
    
    public void SwitchCamera() { 
        //TODO SetCameraState(!fpCam.gameObject.activeSelf);
        SetShipControlsActive(shipCam.gameObject.activeSelf);
    }

    private void SetCameraState(bool fpActive) {
        character.GetCurrentCamera().gameObject.SetActive(fpActive);
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

    public SolarSystemManager GetSolarSystem() {
        return solarSystem;
    }
}

public enum EnumControlType {
    DROID,
    SHIP
}

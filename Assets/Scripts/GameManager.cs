using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager> {

    [SerializeField] private Camera _shipCam;
    [SerializeField] private CharacterController _character;
    [SerializeField] private ShipMoveController _shipMoveController;
    [SerializeField] private SolarSystemManager _solarSystem;
    
    [SerializeField] private Item _emptyItem;

    private DroidManager _droidManager;

    public const float MouseSensitivity = 3.5f;

    //An empty item with no logic, model or assets, used instead of null for missing items or unoccupied scrollbar slots
    public Item EmptyItem => _emptyItem;
    public DroidManager GetDroidManager => _droidManager;

    void Start() {
        _droidManager = GetComponent<DroidManager>();
    }
    
    public void SwitchCamera() { 
        //TODO SetCameraState(!fpCam.gameObject.activeSelf);
        SetShipControlsActive(_shipCam.gameObject.activeSelf);
    }

    private void SetCameraState(bool fpActive) {
        _shipCam.gameObject.SetActive(!fpActive);
        _character.SetPlayerActive(fpActive);
    }

    private void SetShipControlsActive(bool active) {
        _shipMoveController.SetActive(active);
    }

    public void SwitchControlType(EnumControlType controlType) {
        switch (controlType) {
            case EnumControlType.DROID:
                Debug.Log("Switching to droid");
                GetPlayerInput().SwitchCurrentActionMap("General");
                _character.SetActive(true);
                break;
            case EnumControlType.SHIP:
                GetPlayerInput().SwitchCurrentActionMap("Ship");
                _shipMoveController.SetActive(true);
                break;
        }
    }

    public PlayerInput GetPlayerInput() {
        return _character.GetComponent<PlayerInput>();
    }
    
    public ShipMoveController GetShipController() => _shipMoveController;

    public SolarSystemManager GetSolarSystem() {
        return _solarSystem;
    }
}

public enum EnumControlType {
    DROID, //Any droid within controllable range
    SHIP, //The ship itself, to move around
    AI, //The AI, for bridge control and overviews
    NETWORK //The AI, for rapid machine control
}

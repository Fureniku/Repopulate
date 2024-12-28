using Repopulate.Inventory;
using Repopulate.Player;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using Repopulate.World.Celestial;
using Repopulate.Utils.Registries;
using Repopulate.World.Station;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterController = Repopulate.Player.CharacterController;

public class GameManager : MonoSingleton<GameManager> {

    [Header("Ship")]
    [SerializeField] private Camera _shipCam;
    [SerializeField] private ShipMoveController _shipMoveController;

    [Header("Droids")]
    [SerializeField] private CharacterController _character;
    [SerializeField] private DroidManager _droidManager;
    [SerializeField] private PreviewConstruct _previewConstruct;
    
    [Header("World")]
    [SerializeField] private SolarSystemManager _solarSystem;

    public const float MouseSensitivity = 3.5f;

    //An empty construct with no logic, model or assets, used instead of null for missing constructs or unoccupied scrollbar slots
    public Construct EmptyConstruct => ConstructRegistry.Instance.EMPTY;
    public DroidManager GetDroidManager => _droidManager;

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

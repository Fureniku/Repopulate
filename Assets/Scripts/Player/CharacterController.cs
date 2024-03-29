using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour {
    
    [Tooltip("The first-person camera, which should be a child of the controller.")]
    [SerializeField] private Camera fpCam;

    [SerializeField] private float mouseSensitivity = 100.0f;

    [Header("Gameplay stuff")]
    [SerializeField] private DroidController currentDroid;
    [SerializeField] private UIController uiController;

    [SerializeField] private PlayerInput input;
    

    private bool isPlayerActive = true; //True when player is active and can move, false when in a UI or similar.
    
    private float xRotation = 0.0f;
    private int currentDroidId = 0;

    public DroidController GetCurrentDroid() => currentDroid;

    public void SetDroid(DroidController newDroid) {
        currentDroid = newDroid;
        fpCam = newDroid.GetCamera();
        transform.SetParent(newDroid.transform, false);
    }
    
    public void HandleSwitch(InputAction.CallbackContext context) {
        SetActive(false);
        GameManager.Instance.SwitchControlType(EnumControlType.SHIP);
    }

    private void Awake() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        currentDroid.SetDroidActive(true);
    }

    public void SetPlayerActive(bool active) {
        UnityEngine.Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        isPlayerActive = active;
    }

    public void SetActive(bool active) {
        Debug.Log($"Droid active: {active}");
        fpCam.gameObject.SetActive(active);
        currentDroid.SetDroidActive(active);
    }

    public void HandleActiveToggle(InputAction.CallbackContext context) {
        SetPlayerActive(!isPlayerActive);
    }

    public void HandleMovement(InputAction.CallbackContext context) {
        currentDroid.HandleMovement(context.ReadValue<Vector2>());
    }

    public void HandleVerticalMovement(InputAction.CallbackContext context) {
        currentDroid.HandleVerticalMovement(context.ReadValue<float>());
    }

    public Camera GetCurrentCamera() {
        return fpCam;
    }

    public void HandleSwitchDroid(InputAction.CallbackContext context) {
        GameManager.Instance.GetDroidManager.AssignNextAvailableDroid(this);
    }

    public void HandleObjectRotation(InputAction.CallbackContext context) {
        currentDroid.HandleObjectRotation();
    }

    public void HandleJump(InputAction.CallbackContext context) {
        currentDroid.HandleJump();
    }

    public void HandleStabilisation(InputAction.CallbackContext context) {
        currentDroid.HandleStabilisation();
    }
    
    public void HandleCamera(InputAction.CallbackContext context) {
        if (!isPlayerActive || UnityEngine.Cursor.lockState != CursorLockMode.Locked) return;
        Vector2 input = context.ReadValue<Vector2>();
        
        float mouseX = input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        
        if (currentDroid.isInGravity) {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            fpCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            currentDroid.UpdateRotation(Vector3.up * mouseX);
        } else {
            currentDroid.transform.localRotation *= Quaternion.Euler(-mouseY, mouseX, 0f);

            // Set the camera to always look in the direction of Object A's forward vector.
            fpCam.transform.localRotation = Quaternion.identity;
        }
        
        currentDroid.UpdatePreview();
    }

    public void ResetCamera() {
        fpCam.transform.localRotation = Quaternion.identity;
    }

    public void HandleScroll() {
        
    }

    public void HandleUIInput(InputAction.CallbackContext context) {
        currentDroid.HandleUIControlInput(context);
    }
    
    public void HandlePlaceObject(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.LogWarning($"Starting placement from {transform.name} (right click triggered)");
            if (currentDroid.GetPreviewItem().IsPlaceable()) {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = fpCam.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Constants.MASK_BUILDABLE)) {
                    Debug.Log($"Attempting to place a block in the grid after clicking {hit.transform.name}");
                    Direction dir = ColliderUtilities.GetHitFace(hit.normal);
                    BuildingGrid targetGrid = hit.transform.GetComponent<BuildableBase>().GetGrid();
    
                    if (targetGrid != null) {
                        PlaceBlock(targetGrid, targetGrid.GetHitSpace(hit.point), dir);
                    }
                }
            }
        }
    }

    private void PlaceBlock(BuildingGrid targetGrid, Vector3Int gridPosition, Direction dir) {
        //Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {

            //Debug.Log($"GridPos: {gridPosition.x}, {gridPosition.y}, {gridPosition.z}, name of hit object: {hit.transform.name}, exact hit: {hit.point}");

            // Check if there's already a block at the target grid position
            //bool isOccupied = targetGrid.CheckGridSpaceAvailability(gridPosition, Vector3Int.one);

            // Check if there's an adjacent block in any direction
            //bool hasAdjacentBlock = targetGrid.HasAdjacentBlock(gridPosition);

            //if (isOccupied && hasAdjacentBlock)
            {
                // Place against the side or top of an existing block
                //Vector3Int adjacentPosition = targetGrid.GetAdjacentPosition(gridPosition);
                //finalPosition = targetGrid.GridToWorldPosition(adjacentPosition) + Vector3.up;
            }

            // Place the block
            targetGrid.TryPlaceBlock(gridPosition, currentDroid.GetHeldItem(), currentDroid.GetHeldRotation(), dir);
       //}
       //Update the preview after placing a block
       currentDroid.UpdatePreview();
    }
}
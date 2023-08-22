using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour {
    
    [Tooltip("The first-person camera, which should be a child of the controller.")]
    [SerializeField] private Camera fpCam;

    [SerializeField] private float mouseSensitivity = 100.0f;

    [Header("Gameplay stuff")]
    [SerializeField] private DroidController currentDroid;
    [SerializeField] private UIController uiController;
    [SerializeField] private List<GameObject> droidList;

    [SerializeField] private KeyCode SwitchDroidKey;

    private bool isPlayerActive = true; //True when player is active and can move, false when in a UI or similar.
    
    private float xRotation = 0.0f;
    private int currentDroidId = 0;

    private float forwardInput { get; set; }
    private float strafeInput { get; set; }
    private bool jumpInput { get; set; }
    private bool sprintInput { get; set; }

    private void Awake() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        currentDroid.SetDroidActive(true);
    }

    private void SwitchDroid() {
        currentDroid.SetDroidActive(false);
        if (currentDroidId < droidList.Count-1) {
            currentDroidId++;
        } else {
            currentDroidId = 0;
        }

        transform.SetParent(droidList[currentDroidId].transform, false);
        transform.localRotation = Quaternion.identity;

        currentDroid.SetDroidActive(true);
    }

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.F3)) {
          //  SetPlayerActive(!isPlayerActive);
        //}

        if (isPlayerActive) {
            HandleCamera();

            /*if (Input.GetKeyDown(SwitchDroidKey)) {
                SwitchDroid();
            }*/
        }
        
        
        //if (Input.GetMouseButtonDown(1)) {
          //  PlaceSelection();
        //}
        
        //currentDroid.UpdatePreview(fpCam);
        
    }

    public void SetPlayerActive(bool active) {
        UnityEngine.Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        isPlayerActive = active;
    }

    private void FixedUpdate() {
        //forwardInput = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        //strafeInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        //sprintInput = Input.GetKey(KeyCode.LeftShift);
    }
    
    private void HandleCamera() {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked) {
            float mouseX = 0;//Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = 0;//Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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
        }
    }

    public void ResetCamera() {
        fpCam.transform.localRotation = Quaternion.identity;
    }

    void PlaceSelection() {
        Debug.Log("Starting placement");
        if (currentDroid.GetPreviewItem().IsPlaceable()) {
            Debug.Log("its placeable!");
            Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
                BuildingGrid targetGrid = hit.transform.GetComponent<GridCollider>().GetGrid();
    
                if (targetGrid != null) {
                    PlaceBlock(targetGrid, targetGrid.GetHitSpace(hit.point));
                }
            }
        }
    }

    private void PlaceBlock(BuildingGrid targetGrid, Vector3Int gridPosition) {
        Debug.Log("why is all this commented out i really need to clean up my code");
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
            targetGrid.PlaceBlock(gridPosition, currentDroid.GetHeldItem(), currentDroid.GetHeldRotation());
       // }
    }
}
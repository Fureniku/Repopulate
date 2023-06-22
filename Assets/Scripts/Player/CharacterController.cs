using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour {

    [Header("Player controller settings")]
    [Tooltip("Maximum slope the character can jump on")]
    [Range(5f, 60f)]
    [SerializeField] private float slopeLimit = 45f;
    
    [Tooltip("Move speed in meters/second")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Tooltip("Multiplier on forward move speed when sprinting")]
    [SerializeField] private float sprintFactor = 2f;

    [Tooltip("Upward speed to apply when jumping in meters/second")]
    [SerializeField] private float jumpSpeed = 4f;

    [Tooltip("The first-person camera, which should be a child of the controller.")]
    [SerializeField] private Camera fpCam;

    [SerializeField] private float mouseSensitivity = 100.0f;

    [Header("Gameplay stuff")]
    [SerializeField] private Scrollbar scrollbar;
    //[SerializeField] private ScrollBarHandler scrollBarHandler;
    [SerializeField] private UIController uiController;
    [SerializeField] private List<GameObject> droidList;

    [SerializeField] private KeyCode SwitchDroidKey;

    [SerializeField] private GameObject heldItem;
    [SerializeField] private float heldRotation = 0;

    [SerializeField] private Material previewMaterialValid;
    [SerializeField] private Material previewMaterialInvalid;

    private bool isPlayerActive = true; //True when player is active and can move, false when in a UI or similar.
    private bool canPlaceNow = true; //Updated with the preview, changes the material shading and also allows/disallows placement.
    private float xRotation = 0.0f;
    private int currentDroidId = 0;

    private bool IsGrounded { get; set; }
    private float forwardInput { get; set; }
    private float strafeInput { get; set; }
    private bool jumpInput { get; set; }
    private bool sprintInput { get; set; }
    
    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;

    public void UpdateSelection() {
        DestroyImmediate(heldItem);
        heldItem = Instantiate(scrollbar.GetHeldItem().Get());//blockPrefab[scrollBarHandler.GetSelectedSlot()]);
        heldItem.GetComponent<PlaceableObject>().UpdateMaterials(previewMaterialValid);
    }

    private void Awake() {
        Transform parent = transform.parent;
        rigidbody = parent.GetComponent<Rigidbody>();
        capsuleCollider = parent.GetComponent<CapsuleCollider>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        heldItem = Instantiate(scrollbar.GetHeldItem().Get());
        heldItem.GetComponent<PlaceableObject>().UpdateMaterials(previewMaterialValid);
    }

    private void SwitchDroid() {
        if (currentDroidId < droidList.Count-1) {
            currentDroidId++;
        }
        else {
            currentDroidId = 0;
        }

        transform.SetParent(droidList[currentDroidId].transform, false);
        transform.localRotation = Quaternion.identity;

        Transform parent = transform.parent;
        rigidbody = parent.GetComponent<Rigidbody>();
        capsuleCollider = parent.GetComponent<CapsuleCollider>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F3)) {
            SetPlayerActive(!isPlayerActive);
        }

        if (isPlayerActive) {
            if (Input.GetKeyDown(KeyCode.R)) {
                HandleRotation();
            }

            HandleCamera();

            if (Input.GetKeyDown(SwitchDroidKey)) {
                SwitchDroid();
            }
        }
        
        
        if (Input.GetMouseButtonDown(1)) {
            PlaceSelection();
        }
        
        UpdatePreview();
    }

    public void SetPlayerActive(bool active) {
        UnityEngine.Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        isPlayerActive = active;
    }

    private void HandleRotation() {
        heldRotation += 90f;
        if (heldRotation > 360) heldRotation -= 360f;
    }

    private void FixedUpdate() {
        forwardInput = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        strafeInput = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        sprintInput = Input.GetKey(KeyCode.LeftShift);
        jumpInput = Input.GetKey(KeyCode.Space);

        CheckGrounded();
        ProcessActions();
    }

    private void HandleCamera() {
        if (UnityEngine.Cursor.lockState == CursorLockMode.Locked) {
            float mouseX = 0;
            float mouseY = 0;
        
            if (fpCam.gameObject.activeSelf) {
                mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            } else {
                mouseX = Input.GetAxis("Horizontal") * mouseSensitivity * Time.deltaTime;
            }
        
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            fpCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void UpdatePreview() {
        if (heldItem == null) {
            return;
        }
        Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
            BuildingGrid targetGrid;
            PlaceableObject placeable = heldItem.GetComponent<PlaceableObject>();
            if (hit.transform.parent.GetComponent<PlaceableObject>() != null) {
                targetGrid = hit.transform.parent.GetComponent<PlaceableObject>().GetParentGrid();
            } else {
                targetGrid = hit.transform.parent.GetComponent<BuildingGrid>();
            }
            Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
            placeable.SetRotation(heldRotation + targetGrid.transform.rotation.y);
            heldItem.transform.rotation = targetGrid.GetPreviewRotation();
            heldItem.transform.position = targetGrid.GetPreviewPosition(gridPosition);
            canPlaceNow = targetGrid.CheckGridSpaceAvailability(gridPosition, placeable.GetItem().GetSize());

            if (placeable.GetItem().MustBeGrounded() && gridPosition.y > 0) {
                canPlaceNow = false;
            }
            
            placeable.UpdateMaterials(canPlaceNow ? previewMaterialValid : previewMaterialInvalid);
        }
    }
    

    void PlaceSelection() {
        if (canPlaceNow) {
            Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
                BuildingGrid targetGrid = hit.transform.parent.GetComponent<BuildingGrid>();
    
                if (targetGrid != null) {
                    PlaceBlock(targetGrid);
                }
            }
        }
    }
    
    private void PlaceBlock(BuildingGrid targetGrid)
    {
        Ray ray = fpCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("BuildingGrid"))) {
            
            Vector3Int gridPosition = targetGrid.GetHitSpace(hit.point);
            Debug.Log($"GridPos: {gridPosition.x}, {gridPosition.y}, {gridPosition.z}, name of hit object: {hit.transform.name}, exact hit: {hit.point}");

            // Check if there's already a block at the target grid position
            bool isOccupied = targetGrid.CheckGridSpaceAvailability(gridPosition, Vector3Int.one);

            // Check if there's an adjacent block in any direction
            //bool hasAdjacentBlock = targetGrid.HasAdjacentBlock(gridPosition);

            //if (isOccupied && hasAdjacentBlock)
            {
                // Place against the side or top of an existing block
                //Vector3Int adjacentPosition = targetGrid.GetAdjacentPosition(gridPosition);
                //finalPosition = targetGrid.GridToWorldPosition(adjacentPosition) + Vector3.up;
            }

            // Place the block
            targetGrid.PlaceBlock(gridPosition, scrollbar.GetHeldItem().Get());
        }
    }
    
    //Check if the character is touching the floor in some way.
    private void CheckGrounded() {
        IsGrounded = false;
        float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
        Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;
        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up); //Cast a ray from the bottom of the characters capsule
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f)) {
            float normalAngle = Vector3.Angle(hit.normal, transform.up);
            if (normalAngle < slopeLimit) {
                float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
                if (hit.distance < maxDist) {
                    IsGrounded = true;
                }
            }
        }
    }

    //Handle movement and abilities
    private void ProcessActions() {
        //Process Movement/Jumping
        Vector3 velForward = transform.forward * Mathf.Clamp(forwardInput, -1f, 1f);
        Vector3 velStrafe = transform.right * Mathf.Clamp(strafeInput, -1f, 1f);

        if (forwardInput > 0 && sprintInput) {
            velForward *= sprintFactor;
        }
        
        if (IsGrounded) {
            rigidbody.velocity = Vector3.zero; //Reset the velocity
            if (jumpInput) { //Check if trying to jump
                rigidbody.velocity += Vector3.up * jumpSpeed; //Apply an upward velocity to jump
            }

            //Process movement
            rigidbody.velocity += velForward * moveSpeed;
            rigidbody.velocity += velStrafe * moveSpeed;
        } else {
            // Check if player is trying to change forward/backward movement while jumping/falling
            if (!Mathf.Approximately(forwardInput, 0f)) {
                // Override just the forward velocity with player input at half speed
                Vector3 verticalVelocity = Vector3.Project(rigidbody.velocity, Vector3.up);
                rigidbody.velocity = verticalVelocity + velForward * moveSpeed / 2f + velStrafe * moveSpeed / 2f;
            }
        }
    }
}
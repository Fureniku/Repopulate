using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroidController : PlayerControllable {
	
	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float heldRotation = 0;
	[SerializeField] private UIController _uiController;
	
	[SerializeField] private Rigidbody rb;
	[SerializeField] private Transform footPoint;

	[SerializeField] private InventoryManager _inventory;

	[Header("Control settings")]
	[Tooltip("Realistic space controls with momentum. Set to false for precise transform controls.")]
	[SerializeField] private bool spaceControls;
	[Tooltip("Move speed in meters/second")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float moveSpeedSpace = 25f;
	//[Tooltip("Multiplier on forward move speed when sprinting")]
	//[SerializeField] private float sprintFactor = 2f;
	//[Tooltip("Maximum slope the character can jump on")]
	//[Range(5f, 60f)]
	//[SerializeField] private float slopeLimit = 45f;
	[Tooltip("Upward speed to apply when jumping in meters/second")]
	[SerializeField] private float jumpSpeed = 4f;
	[Tooltip("The maximum velocity the droid can move at")]
	[SerializeField] private float maxSpeed = 5f;

	private GravityAffectedObject _gao;
	
	private Vector3 moveDir; //The in-gravity movement input

	//In-space variables
	private Vector3 currentMomentum;
	private Vector3 currentDirection;
	
	[SerializeField] private float groundedDistance = 2f;

	public int forcedNotGroundedCount { get; private set; } = 0; //Allows overlap of forced grounded areas without having to make a whole list of them.
	public bool forcedNotGrounded { get; private set; } = false;
	public bool isGrounded { get; private set; } = false;
	public Vector3 MoveDir => moveDir;
	public GravityAffectedObject DroidGao => _gao;
	public GravityBase CurrentGravitySource => _gao.GravitySource;
	public InventoryManager DroidInventory => _inventory;
	
	private bool isControlActive = true; //Whether the controls (movement/look) are currently active. Disabled while a UI is open etc
	private bool isDroidActive = false; //Whether this droid currently has a controller TODO improve

	public bool IsDroidActive => isDroidActive;

	public bool grounded;
	
	protected override void ControllableAwake() {
		UpdateSelection();
		rb = GetComponent<Rigidbody>();
		_gao = GetComponent<GravityAffectedObject>();
	}

	protected override void ControllableFixedUpdate() {
		if (!isControlActive) {
			return;
		}
		CheckGrounded();
		Movement();
		grounded = isGrounded;
	}
	
	#region Movement controls
	
	public void HandleMovement(Vector2 input) {
		moveDir.x = input.x;
		moveDir.z = input.y;
	}

	public void HandleVerticalMovement(float input) {
		moveDir.y = _gao.IsInGravity ? 0 : input;
	}

	public void HandleObjectRotation() {
		if (isDroidActive) {
			heldRotation += 90f;
			if (heldRotation > 360) heldRotation -= 360f;
		}
	}

	public void HandleJump() {
		if (isGrounded && _gao.IsInGravity && isControlActive) {
			Debug.Log("Jump!");
			rb.AddForce(transform.TransformDirection(Vector3.up) * jumpSpeed, ForceMode.Impulse);
		}
	}

	public void HandleStabilisation() {
		rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
	}

	private readonly bool physicsMovement = true; //Debug switch to use physics addforce or transform.translate to move character on X
	
	//Handle movement and abilities
	private void Movement() {
		Transform t = transform;
		Vector3 moveDirection = Vector3.zero;
		moveDirection += t.forward * moveDir.z;
		moveDirection += t.right * moveDir.x;
		moveDirection += t.up * moveDir.y;

		if (_gao.IsInGravity) {
			//Store our Y velocity so it's unaffected by player movement on X/Z
			float velocityY = rb.velocity.y;

			//Player input movement:
			//Apply a velocity change force to instantly move them
			float groundedModifier = isGrounded ? 10f : 5f; //Less movement when in the air
			if (physicsMovement) {
				rb.AddForce(moveDirection * (moveSpeed * Time.deltaTime * groundedModifier), ForceMode.VelocityChange);
			} else {
				transform.Translate(moveDir * (moveSpeed * Time.deltaTime * groundedModifier));
			}
			//Restore the Y velocity
			rb.velocity = new Vector3(rb.velocity.x, velocityY, rb.velocity.z);

			//Next, apply the gravitational force, pulling the object down.
			_gao.UpdateGravity();
			
			//Now, if they've stopped pressing input and are on the floor, stop moving. Don't stop vertical velocity. Must be done in local space!
			if (moveDir == Vector3.zero && isGrounded) {
				rb.velocity = new Vector3(0, velocityY, 0);
				
				Vector3 localDirection = rb.transform.InverseTransformDirection(rb.velocity);

				localDirection.x = 0f;
				localDirection.z = 0f;

				Vector3 globalDirection = rb.transform.TransformDirection(localDirection);

				rb.velocity = globalDirection;
			}

			//Finally, limit velocities within maximum ranges
			rb.ClampVelocity(maxSpeed);
		} else {
			// Check for input to activate the thrusters
			moveDirection += t.forward * moveDir.z;
			moveDirection += t.right * moveDir.x;
			moveDirection += t.up * moveDir.y;
			
			// Apply the relative movement
			rb.AddForce(moveDirection.normalized * (moveSpeedSpace * Time.deltaTime * 10));
		}
	}

	public void HandleUIControlInput(InputAction.CallbackContext context) {
		_uiController.ScrollBar.HandleUIInput(context);
	}

	public void ForceNotGroundedState(bool forced) {
		if (forced) {
			forcedNotGroundedCount++;
		} else {
			forcedNotGroundedCount--;
		}
		
		forcedNotGrounded = forcedNotGroundedCount > 0;
	}

	//Check if the character is touching the floor in some way, while within gravity
	private void CheckGrounded() {
		isGrounded = false;
		if (forcedNotGrounded || !_gao.IsInGravity) {
			return;
		}

		LayerMask layerMask = Constants.MASK_STANDABLE;
		Collider[] hitColliders = Physics.OverlapSphere(footPoint.position, 2f, layerMask);
		int colliderCount = hitColliders.Length;
		for (int i = 0; i < colliderCount; i++) {
			Collider col = hitColliders[i];
			Vector3 footPos = footPoint.position;
			if (Vector3.Distance(footPos, col.ClosestPoint(footPos)) < groundedDistance) {
				isGrounded = true;
			}
		}
	}
	#endregion

	#region Block Placement
	public void UpdateSelection() {
		if (heldItem == null) {
			Debug.LogWarning($"Trying to update held item for {name} but held item object is null.");
			return;
		}

		if (scrollbar == null) {
			Debug.LogWarning($"Trying to update selection for {name} but scrollbar is null");
			return;
		}

		if (scrollbar.GetHeldItem() == null) {
			Debug.LogWarning($"Trying to update selection with {scrollbar.name} but nothing is selected");
			return;
		}

		Debug.Log($"UpdateSelection! helditem name; {heldItem.name}");
		Debug.Log($"UpdateSelection! scrollbar name: {scrollbar.name}");
		Debug.Log($"UpdateSelection! scrollbar selection: {scrollbar.GetHeldItem().name}");
		Debug.Log($"UpdateSelection! scrollbar item SO: {scrollbar.GetHeldItem().Get().name}");
		
		heldItem.SetObject(scrollbar.GetHeldItem());
	}

	public void UpdatePreview() {
		heldItem.UpdatePreview(_camera);
	}

	public PreviewItem GetPreviewItem() {
		return heldItem;
	}

	public Item GetHeldItem() {
		return scrollbar.GetHeldItem();
	}

	public float GetHeldRotation() {
		return heldRotation;
	}
	
	public void HandlePlaceObject(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.LogWarning($"Starting placement from {transform.name} (right click triggered)");
            if (heldItem.IsPlaceable()) {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = _camera.ScreenPointToRay(mousePosition);
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
            targetGrid.TryPlaceBlock(gridPosition, scrollbar.GetHeldItem(), heldRotation, dir);
       //}
       //Update the preview after placing a block
       UpdatePreview();
    }
	#endregion
	
	#region Camera
	private float xRotation = 0.0f;
	
	public void HandleCamera(InputAction.CallbackContext context) {
		Vector2 input = context.ReadValue<Vector2>();
        
		float mouseX = input.x * GameManager.MouseSensitivity * Time.deltaTime;
		float mouseY = input.y * GameManager.MouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
        
		if (_gao.IsInGravity) {
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			_camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			UpdateRotation(Vector3.up * mouseX);
		} else {
			transform.localRotation *= Quaternion.Euler(-mouseY, mouseX, 0f);

			// Set the camera to always look in the direction of Object A's forward vector.
			_camera.transform.localRotation = Quaternion.identity;
		}
        
		UpdatePreview();
	}
	#endregion

	public void SetControlsActive(bool active) {
		isControlActive = active;
	}
	
	public void SetDroidActive(bool active) {
		isDroidActive = active;
		_camera.gameObject.SetActive(active);
	}

	public void UpdateRotation(Vector3 newRot) {
		transform.Rotate(newRot);
	}
	
	#region Inventory
	public int Give(Resource resource, int count) {
		return _inventory.InsertResource(resource, count);
	}
	
	public void InventoryVisible(bool visible) {
		_uiController.InventoryVisible(visible);
	}
	#endregion

	#region Debug
	public void OnDrawGizmos() {
		Gizmos.DrawSphere(footPoint.position, 0.5f);
	}
	#endregion
}

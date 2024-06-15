using Repopulate.Inventory;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using Repopulate.World.Constructs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Repopulate.Player {
	public class DroidController : PlayerControllable {
	
		[Header("References")]
		[Tooltip("The gameobject holding the preview functionality")]
		[SerializeField] private PreviewConstruct _previewConstruct;
		[Tooltip("The serialized object for the scrollbar this droid uses")]
		[SerializeField] private Scrollbar _scrollbar;
		[Tooltip("The UI controller")] //TODO still used?
		[SerializeField] private UIController _uiController;
		[Tooltip("The point which is considered the base of the droid, used for grounded checks")]
		[SerializeField] private Transform _footPoint;
		[Tooltip("The droid's inventory system")]
		[SerializeField] private InventoryManager _inventory;
		
		[Header("Control settings")]
		[Tooltip("Realistic space controls with momentum. Set to false for precise transform controls.")]
		[SerializeField] private bool _spaceControls;
		[Tooltip("Move speed in meters/second")]
		[SerializeField] private float _moveSpeed = 5f;
		[SerializeField] private float _moveSpeedSpace = 25f;
		//[Tooltip("Multiplier on forward move speed when sprinting")]
		//[SerializeField] private float _sprintFactor = 2f;
		//[Tooltip("Maximum slope the character can jump on")]
		//[Range(5f, 60f)]
		//[SerializeField] private float _slopeLimit = 45f;
		[Tooltip("Upward speed to apply when jumping in meters/second")]
		[SerializeField] private float _jumpSpeed = 2f;
		[Tooltip("The maximum velocity the droid can move at")]
		[SerializeField] private float _maxSpeed = 10f;
		[Tooltip("The distance between the footpoint and the floor to be considered \"grounded\"")]
		[SerializeField] private float _groundedDistance = 0.6f;
		
		[SerializeField] private bool _physicsMovement = true; //Debug switch to use physics addforce or transform.translate to move character on X

		private GravityAffectedObject _gao;
		private Rigidbody _rigidbody;
		private Vector3 _moveDir; //The in-gravity movement input
		private float _heldRotation = 0;
		private float _cameraXRotation = 0.0f; //camera stuff

		public int ForcedNotGroundedCount { get; private set; } = 0; //Allows overlap of forced grounded areas without having to make a whole list of them.
		public bool ForcedNotGrounded => ForcedNotGroundedCount > 0;
		public bool IsGrounded { get; private set; } = false;
		public Vector3 MoveDir => _moveDir;
		public GravityAffectedObject DroidGao => _gao;
		public GravityBase CurrentGravitySource => _gao.GravitySource;
		public InventoryManager DroidInventory => _inventory;
		public PreviewConstruct PreviewConstruct => _previewConstruct;
		public Construct SelectedConstruct => _scrollbar.GetSelectedConstruct();
		public float HeldRotation => _heldRotation;
		public bool IsControlActive { get; set; } = true; //Whether the controls (movement/look) are currently active. Disabled while a UI is open etc
		public bool IsDroidActive { get; private set; } = false; //Whether this droid currently has a controller TODO improve

		protected override void ControllableAwake() {
			UpdateSelection();
			_rigidbody = GetComponent<Rigidbody>();
			_gao = GetComponent<GravityAffectedObject>();
		}

		protected override void ControllableFixedUpdate() {
			if (!IsControlActive) {
				return;
			}
			CheckGrounded();
			Movement();
		}
	
		#region Movement controls
	
		public void HandleMovement(Vector2 input) {
			_moveDir.x = input.x;
			_moveDir.z = input.y;
		}

		public void HandleVerticalMovement(float input) {
			_moveDir.y = _gao.IsInGravity ? 0 : input;
		}

		public void HandleObjectRotation() {
			if (IsDroidActive) {
				_heldRotation += 90f;
				if (_heldRotation > 360) _heldRotation -= 360f;
			}
		}

		public void HandleJump() {
			if (IsGrounded && _gao.IsInGravity && IsControlActive) {
				Debug.Log("Jump!");
				_rigidbody.AddForce(transform.TransformDirection(Vector3.up) * _jumpSpeed, ForceMode.Impulse);
			}
		}

		public void HandleStabilisation() {
			_rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
		}

		//Handle movement and abilities
		private void Movement() {
			Transform t = transform;
			Vector3 moveDirection = Vector3.zero;
			moveDirection += t.forward * _moveDir.z;
			moveDirection += t.right * _moveDir.x;
			moveDirection += t.up * _moveDir.y;

			if (_gao.IsInGravity) {
				//Store our Y velocity so it's unaffected by player movement on X/Z
				float velocityY = _rigidbody.velocity.y;

				//Player input movement:
				//Apply a velocity change force to instantly move them
				float groundedModifier = IsGrounded ? 10f : 5f; //Less movement when in the air
				if (_physicsMovement) {
					_rigidbody.AddForce(moveDirection * (_moveSpeed * Time.deltaTime * groundedModifier), ForceMode.VelocityChange);
				} else {
					transform.Translate(_moveDir * (_moveSpeed * Time.deltaTime * groundedModifier));
				}
				//Restore the Y velocity
				_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, velocityY, _rigidbody.velocity.z);

				//Next, apply the gravitational force, pulling the object down.
				_gao.UpdateGravity();
			
				//Now, if they've stopped pressing input and are on the floor, stop moving. Don't stop vertical velocity. Must be done in local space!
				if (_moveDir == Vector3.zero && IsGrounded) {
					_rigidbody.velocity = new Vector3(0, velocityY, 0);
				
					Vector3 localDirection = _rigidbody.transform.InverseTransformDirection(_rigidbody.velocity);

					localDirection.x = 0f;
					localDirection.z = 0f;

					Vector3 globalDirection = _rigidbody.transform.TransformDirection(localDirection);

					_rigidbody.velocity = globalDirection;
				}

				//Finally, limit velocities within maximum ranges
				_rigidbody.ClampVelocity(_maxSpeed);
			} else {
				// Check for input to activate the thrusters
				moveDirection += t.forward * _moveDir.z;
				moveDirection += t.right * _moveDir.x;
				moveDirection += t.up * _moveDir.y;
			
				// Apply the relative movement
				_rigidbody.AddForce(moveDirection.normalized * (_moveSpeedSpace * Time.deltaTime * 10));
			}
		}

		public void HandleUIControlInput(InputAction.CallbackContext context) {
			_uiController.ScrollBar.HandleUIInput(context);
		}

		public void ForceNotGroundedState(bool forced) {
			if (forced) {
				ForcedNotGroundedCount++;
			} else {
				ForcedNotGroundedCount--;
			}
		}

		//Check if the character is touching the floor in some way, while within gravity
		private void CheckGrounded() {
			IsGrounded = false;
			if (ForcedNotGrounded || !_gao.IsInGravity) {
				return;
			}

			LayerMask layerMask = Constants.MASK_STANDABLE;
			Collider[] hitColliders = Physics.OverlapSphere(_footPoint.position, 2f, layerMask);
			int colliderCount = hitColliders.Length;
			for (int i = 0; i < colliderCount; i++) {
				Collider col = hitColliders[i];
				Vector3 footPos = _footPoint.position;
				if (Vector3.Distance(footPos, col.ClosestPoint(footPos)) < _groundedDistance) {
					IsGrounded = true;
				}
			}
		}
		#endregion

		#region Block Placement
		private void UpdateSelection() {
			if (_previewConstruct == null) {
				Debug.LogWarning($"Trying to update held item for {name} but held item object is null.");
				return;
			}

			if (_scrollbar == null) {
				Debug.LogWarning($"Trying to update selection for {name} but scrollbar is null");
				return;
			}

			if (_scrollbar.GetSelectedConstruct() == null) {
				Debug.LogWarning($"Trying to update selection with {_scrollbar.name} but nothing is selected");
				return;
			}

			Debug.Log($"UpdateSelection! preview construct name; {_previewConstruct.name}");
			Debug.Log($"UpdateSelection! scrollbar name: {_scrollbar.name}");
			Debug.Log($"UpdateSelection! scrollbar selection: {_scrollbar.GetSelectedConstruct().name}");
			Debug.Log($"UpdateSelection! scrollbar item SO: {_scrollbar.GetSelectedConstruct().Get().name}");
		
			_previewConstruct.SetObject(_scrollbar.GetSelectedConstruct());
		}

		public void HandlePlaceObject(InputAction.CallbackContext context) {
			Debug.LogWarning($"Starting placement from {transform.name} (right click triggered)");
			if (_previewConstruct.IsPlaceable()) {
				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Ray ray = _camera.ScreenPointToRay(mousePosition);
				if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Constants.MASK_BUILDABLE)) {
					Debug.Log($"Attempting to place a block in the grid after clicking {hit.transform.name}");
					Direction dir = ColliderUtilities.GetHitFace(hit.normal);
					ConstructGrid targetGrid = hit.transform.GetComponent<ConstructBase>().GetGrid();

					if (targetGrid != null) {
						PlaceBlock(targetGrid, targetGrid.GetHitSpace(hit.point), dir);
					}
				}
			}
		}

		private void PlaceBlock(ConstructGrid targetGrid, Vector3Int gridPosition, Direction dir) {
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
			targetGrid.TryPlaceBlock(gridPosition, _scrollbar.GetSelectedConstruct(), _heldRotation, dir);
			//}
			//Update the preview after placing a block
			_previewConstruct.UpdatePreview(_camera);
		}
		#endregion
	
		#region Camera
		public void HandleCamera(InputAction.CallbackContext context) {
			Vector2 input = context.ReadValue<Vector2>();
        
			float mouseX = input.x * GameManager.MouseSensitivity * Time.deltaTime;
			float mouseY = input.y * GameManager.MouseSensitivity * Time.deltaTime;

			_cameraXRotation -= mouseY;
        
			if (_gao.IsInGravity) {
				_cameraXRotation = Mathf.Clamp(_cameraXRotation, -90f, 90f);

				_camera.transform.localRotation = Quaternion.Euler(_cameraXRotation, 0f, 0f);
				UpdateRotation(Vector3.up * mouseX);
			} else {
				transform.localRotation *= Quaternion.Euler(-mouseY, mouseX, 0f);

				// Set the camera to always look in the direction of Object A's forward vector.
				_camera.transform.localRotation = Quaternion.identity;
			}
        
			_previewConstruct.UpdatePreview(_camera);
		}
		#endregion

		public void SetDroidActive(bool active) {
			IsDroidActive = active;
			_camera.gameObject.SetActive(active);
		}

		public void UpdateRotation(Vector3 newRot) {
			transform.Rotate(newRot);
		}
	
		#region Inventory
		public int Give(Item item, int count) {
			return _inventory.InsertItem(item, count);
		}
	
		public void InventoryVisible(bool visible) {
			_uiController.InventoryVisible(visible);
		}
		#endregion

		#region Debug
		public void OnDrawGizmos() {
			Gizmos.DrawSphere(_footPoint.position, 0.5f);
		}
		#endregion
	}
}

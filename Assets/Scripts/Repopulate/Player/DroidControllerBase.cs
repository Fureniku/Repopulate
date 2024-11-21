using Repopulate.Inventory;
using Repopulate.Physics.Gravity;
using Repopulate.ScriptableObjects;
using Repopulate.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Repopulate.Player {
	public abstract class DroidControllerBase : PlayerControllable {

		[SerializeField] private DroidType _droidType;
		[Header("References")]
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
		private float _cameraXRotation = 0.0f; //camera stuff

		public int ForcedNotGroundedCount { get; private set; } = 0; //Allows overlap of forced grounded areas without having to make a whole list of them.
		public bool ForcedNotGrounded => ForcedNotGroundedCount > 0;
		public bool IsGrounded { get; private set; } = false;
		public Vector3 MoveDir => _moveDir;
		public GravityAffectedObject DroidGao => _gao;
		public GravityBase CurrentGravitySource => _gao.GravitySource;
		public InventoryManager DroidInventory => _inventory;
		public bool IsControlActive { get; set; } = true; //Whether the controls (movement/look) are currently active. Disabled while a UI is open etc
		public bool IsDroidActive { get; private set; } = false; //Whether this droid currently has a controller TODO improve
		public DroidType DroidType => _droidType;
		
		//Debug only properties
		public GameObject CurrentAimTarget => _lastObject;

		protected override void ControllableAwake() {
			_rigidbody = GetComponent<Rigidbody>();
			_gao = GetComponent<GravityAffectedObject>();
			DroidAwake();
		}

		#region Subclass stuff
		protected abstract void DroidAwake();
		protected abstract void UpdateDroidCamera();
		
		public abstract void DroidCreativeInput(InputAction.CallbackContext context); // Rightclick: Creative style input for droids which have it (e.g. construction placing a construct)
		public abstract void DroidDestructiveInput(InputAction.CallbackContext context); // Leftclick: Destructive input for droids which have it (e.g. miner mining)
		public abstract void DroidModifierInput(); // R: Modify how your ability works (e.g. rotating the construct for a construction droid)
		#endregion

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
			UpdateDroidCamera();
		}

		public void HandleVerticalMovement(float input) {
			_moveDir.y = _gao.IsInGravity ? 0 : input;
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
			Collider[] hitColliders = UnityEngine.Physics.OverlapSphere(_footPoint.position, 2f, layerMask);
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

			UpdateDroidCamera();
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

public enum DroidType {
	CONSTRUCTION,
	ITEM,
	TRANSPORT,
	REPAIR,
	MANAGEMENT
}

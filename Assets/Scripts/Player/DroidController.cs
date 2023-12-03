using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroidController : MonoBehaviour {

	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float heldRotation = 0;
	
	[SerializeField] private Rigidbody rb;
	[SerializeField] private CapsuleCollider capsuleCollider;

	[SerializeField] private GravityBase gravitySource;

	[SerializeField] private CharacterController characterController;
	
	[SerializeField] private Transform footPoint;

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
	[Tooltip("The speed at which the droid rotates to correct orientation when entering gravity")]
	[Range(1f, 10f)]
	[SerializeField] private float gravitationalCorrectionSpeed = 5f;
	[Tooltip("The maximum velocity the droid can move at")]
	[SerializeField] private float maxSpeed = 5f;
	//[Tooltip("The maximum velocity the droid can move at when moving upwards")]
	//[SerializeField] private float maxSpeedUp = 5f;
	//[Tooltip("The maximum velocity the droid can move at when falling (in gravity)")]
	//[SerializeField] private float maxSpeedFall = 25f;
	//[Tooltip("The maximum velocity the droid can move at (on all axis) while in space or zero-G areas")]
	//[SerializeField] private float maxSpeedZeroG = 5f;

	private Vector3 moveDir; //The in-gravity movement input
	private Vector3 lastPosition; //The last known position when in gravity, used for transitioning velocity to out-of-gravity
	private List<GravityBase> currentGravities = new();
	
	//In-space variables
	private Vector3 currentMomentum;
	private Vector3 currentDirection;
	
	[SerializeField] private float groundedDistance = 2f;

	public int forcedNotGroundedCount { get; private set; } = 0; //Allows overlap of forced grounded areas without having to make a whole list of them.
	public bool forcedNotGrounded { get; private set; } = false;

	public bool isGrounded { get; private set; } = false;
	public bool isInGravity { get; private set; } = false;
	public bool isInElevator { get; private set; } = false;
	private bool isDroidActive = false; //Whether this is the currently controlled droid. Not to be confused with playeractive which locks the camera etc

	public bool grounded;
	
	void Awake() {
		UpdateSelection();
		rb = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		isInGravity = gravitySource != null;
	}
	
	private void FixedUpdate() {
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
		moveDir.y = isInGravity ? 0 : input;
	}

	public void HandleSwitchDroid(InputAction.CallbackContext context) {
		
	}

	public void HandleObjectRotation() {
		if (isDroidActive) {
			heldRotation += 90f;
			if (heldRotation > 360) heldRotation -= 360f;
		}
	}

	public void HandleJump() {
		if (isGrounded && isInGravity) {
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

		if (isInGravity) {
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
			Gravity();
			
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
			ClampVelocity();
		} else {
			// Check for input to activate the thrusters
			moveDirection += t.forward * moveDir.z;
			moveDirection += t.right * moveDir.x;
			moveDirection += t.up * moveDir.y;
			
			// Apply the relative movement
			rb.AddForce(moveDirection.normalized * (moveSpeedSpace * Time.deltaTime * 10));
		}
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
		if (forcedNotGrounded || gravitySource == null) {
			return;
		}

		LayerMask layerMask = LayerMask.GetMask("Floor") | LayerMask.GetMask("Wall");
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

	private void ClampVelocity() {
		Vector3 velocityIn = rb.velocity;
		float currentSpeed = velocityIn.magnitude;
		
		if (currentSpeed > maxSpeed) {
			Vector3 desiredVelocity = rb.velocity.normalized * maxSpeed;
			rb.velocity = desiredVelocity;
		}
	}
	
	private void LateUpdate() {
		if (isInGravity) {
			lastPosition = transform.position;
		}
	}
	#endregion

	#region Item Stuff
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
		
		heldItem.SetObject(scrollbar.GetHeldItem().Get());
	}

	public void UpdatePreview(Camera cam) {
		heldItem.UpdatePreview(cam);
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
	

	#endregion
	
	public void SetDroidActive(bool active) {
		isDroidActive = active;
	}

	public void UpdateRotation(Vector3 newRot) {
		transform.Rotate(newRot);
	}
	
	#region Gravity
	private void Gravity() {
		Vector3 pos = transform.position;
		if (currentGravities.Count > 0) {
			if (!isInGravity) {
				characterController.ResetCamera();
				transform.localScale = Vector3.one;
			}

			GravityBase priorityGravity = currentGravities[0];
			
			foreach (GravityBase grav in currentGravities) {
				if (grav.GetPriority() > priorityGravity.GetPriority()) {
					if (grav.IsWithinGravitationalEffect(pos)) {
						priorityGravity = grav;
					}
				}
			}

			gravitySource = priorityGravity;
			isInGravity = true;
		} else {
			if (isInGravity) {
				gravitySource = null;
				transform.parent = StationController.Instance.transform;
				ClampVelocity();
			}
			isInGravity = false;
		}

		if (gravitySource != null && gravitySource.IsWithinGravitationalEffect(pos)) {
			Vector3 direction = gravitySource.GetPullDirection(pos);
			Vector3 gravDirection = gravitySource.GetPull(pos);
		
			rb.AddForce(gravDirection);

			Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
			Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * gravitationalCorrectionSpeed);

			transform.rotation = slerp;
		}
	}

	public GravityBase CurrentGravitySource() => gravitySource;

	#endregion

	private void OnTriggerEnter(Collider other) {
		GravityBase gravity = other.GetComponent<GravityBase>();

		if (gravity != null) {
			isInGravity = true;
			currentGravities.Add(gravity);
			Debug.Log($"Adding {gravity} to list of gravities. Total now {currentGravities.Count}");
		}
	}

	private void OnTriggerExit(Collider other) {
		GravityBase gravity = other.GetComponent<GravityBase>();

		if (gravity != null) {
			currentGravities.Remove(gravity);
			Debug.Log($"Removing {gravity} from list of gravities. Total now {currentGravities.Count}");
		}
	}

	private void OnTriggerStay(Collider other) {
		GravityLift gravLift = other.GetComponent<GravityLift>();

		if (gravLift != null) {
			isInElevator = true;
			if (moveDir == Vector3.zero) {
				gravLift.HandleForces(rb);
			}
		} else {
			isInElevator = false;
		}
	}

	public void OnDrawGizmos() {
		Gizmos.DrawSphere(footPoint.position, 0.5f);
	}
}

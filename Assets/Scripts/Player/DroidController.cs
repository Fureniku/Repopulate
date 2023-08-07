using System;
using UnityEngine;

public class DroidController : MonoBehaviour {

	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float heldRotation = 0;
	
	[SerializeField] private Rigidbody rb;
	[SerializeField] private CapsuleCollider capsuleCollider;

	[SerializeField] private GravityBase gravitySource;

	[SerializeField] private CharacterController characterController;

	[Header("Control settings")]
	[Tooltip("Realistic space controls with momentum. Set to false for precise transform controls.")]
	[SerializeField] private bool spaceControls;
	[Tooltip("Move speed in meters/second")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float moveSpeedSpace = 25f;
	[Tooltip("Multiplier on forward move speed when sprinting")]
	[SerializeField] private float sprintFactor = 2f;
	[Tooltip("Maximum slope the character can jump on")]
	[Range(5f, 60f)]
	[SerializeField] private float slopeLimit = 45f;
	[Tooltip("Upward speed to apply when jumping in meters/second")]
	[SerializeField] private float jumpSpeed = 4f;
	[Tooltip("The speed at which the droid rotates to correct orientation when entering gravity")]
	[Range(1f, 10f)]
	[SerializeField] private float gravitationalCorrectionSpeed = 5f;
	[Tooltip("The maximum velocity the droid can move at")]
	[SerializeField] private float maxSpeed = 5f;
	[Tooltip("The maximum velocity the droid can move at when moving upwards")]
	[SerializeField] private float maxSpeedUp = 5f;
	[Tooltip("The maximum velocity the droid can move at when falling (in gravity)")]
	[SerializeField] private float maxSpeedFall = 25f;
	[Tooltip("The maximum velocity the droid can move at (on all axis) while in space or zero-G areas")]
	[SerializeField] private float maxSpeedZeroG = 5f;
	
	private Vector3 moveDir; //The in-gravity movement input
	private Vector3 lastPosition; //The last known position when in gravity, used for transitioning velocity to out-of-gravity
	
	//In-space variables
	private Vector3 currentMomentum;
	private Vector3 currentDirection;
	
	private bool isGrounded { get; set; } //Only when in gravity
	public bool isInGravity { get; private set; } = false;
	public bool isInElevator { get; private set; } = false;
	private bool isDroidActive = false; //Whether this is the currently controlled droid. Not to be confused with playeractive which locks the camera etc

	void Awake() {
		UpdateSelection();
		rb = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		isInGravity = gravitySource != null;
	}

	void Update() {
		if (isDroidActive) {
			if (Input.GetKeyDown(KeyCode.R)) {
				heldRotation += 90f;
				if (heldRotation > 360) heldRotation -= 360f;
			}
		}

		float verticalMove = isInGravity ? 0 : GetVerticalMoveAxis();
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), verticalMove, Input.GetAxisRaw("Vertical")).normalized;
	}
	
	private void FixedUpdate() {
		//Gravity();
		CheckGrounded();
		Movement();
	}
	
	#region Movement controls

	private bool physicsMovement = true; //Debug switch to use physics addforce or transform.translate to move character on X
	
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

			//Jump input
			if (isGrounded) {
				if (Input.GetKey(KeyCode.Space)) {
					rb.AddForce(transform.TransformDirection(transform.up) * jumpSpeed, ForceMode.Impulse);
				}
			}
			
			//Finally, limit velocities within maximum ranges
			ClampVelocity();
		} else {
			if (Input.GetKey(KeyCode.E)) { 
				rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
			} else {
				// Check for input to activate the thrusters
				moveDirection += t.forward * moveDir.z;
				moveDirection += t.right * moveDir.x;
				moveDirection += t.up * moveDir.y;
				
				// Apply the relative movement
				rb.AddForce(moveDirection.normalized * (moveSpeedSpace * Time.deltaTime * 10));
			}
		}
	}
	
	private float GetVerticalMoveAxis() {
		float movement = 0;
		if (Input.GetKey(KeyCode.Space)) {
			movement++;
		}

		if (Input.GetKey(KeyCode.LeftControl)) {
			movement--;
		}
		return movement;
	}

	//Check if the character is touching the floor in some way, while within gravity
	private void CheckGrounded() {
		isGrounded = false;
		float capsuleHeight = Mathf.Max(capsuleCollider.radius * 2f, capsuleCollider.height);
		Vector3 capsuleBottom = transform.TransformPoint(capsuleCollider.center - transform.up * capsuleHeight / 2f);
		float radius = transform.TransformVector(capsuleCollider.radius, 0f, 0f).magnitude;
		Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up); //Cast a ray from the bottom of the characters capsule
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, radius * 5f)) {
			float normalAngle = Vector3.Angle(hit.normal, transform.up);
			if (normalAngle < slopeLimit) {
				float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + .02f;
				if (hit.distance < maxDist) {
					isGrounded = true;
				}
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

		if (gravitySource != null && gravitySource.IsWithinGravitationalEffect(pos)) {
			Vector3 direction = gravitySource.GetPullDirection(pos);
			Vector3 gravDirection = gravitySource.GetPull(pos);
		
			rb.AddForce(gravDirection);

			Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
			Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * gravitationalCorrectionSpeed);

			transform.rotation = slerp;
		}
	}

	public void AssignGravityToDroid(GravityBase gravity) {
		isInGravity = true;
		gravitySource = gravity;
		characterController.ResetCamera();
		transform.localScale = Vector3.one;
	}

	public GravityBase CurrentGravitySource() {
		return gravitySource;
	}

	public void ExitGravity() {
		gravitySource = null;
		isInGravity = false;
		transform.parent = StationController.Instance.transform;
		ClampVelocity();
	}

	private void GravityLift() {
		
	}
	
	#endregion

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

	private void OnTriggerExit(Collider other) {
		isInElevator = false;
	}
}

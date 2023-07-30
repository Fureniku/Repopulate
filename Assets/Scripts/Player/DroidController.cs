using UnityEngine;

public class DroidController : MonoBehaviour {

	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float heldRotation = 0;
	
	[SerializeField] private Rigidbody rb;
	[SerializeField] private CapsuleCollider capsuleCollider;

	[SerializeField] private GravityBase gravitySource;
	[SerializeField] private MultiGravitySelector multiGravitySelector;
	
	[Header("Control settings")]
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
	
	private Vector3 moveDir;
	
	private bool isGrounded { get; set; }


	public bool isInGravity { get; private set; }= false;

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

		//TODO temporary MC creative-esque flight out of gravity.
		//Change to physics-based to counter force and drift with a stabilisation key and ability to kick off surfaces later.
		float verticalMove = isInGravity ? 0 : GetVerticalMoveAxis();
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), verticalMove, Input.GetAxisRaw("Vertical")).normalized;
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
	
	private void FixedUpdate() {
		Gravity();
		CheckGrounded();
		Movement();
	}

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

	public void SetDroidActive(bool active) {
		isDroidActive = active;
	}

	public void UpdateRotation(Vector3 newRot) {
		transform.Rotate(newRot);
	}

	private void Gravity() {
		Vector3 pos = transform.position;
		if (multiGravitySelector != null) {
			gravitySource = multiGravitySelector.GetClosestGravity(pos);
		}
		
		if (gravitySource != null && gravitySource.IsWithinGravitationalEffect(pos)) {
			Vector3 direction = gravitySource.GetPullDirection(pos);
			Vector3 gravDirection = gravitySource.GetPull(pos);
		
			rb.AddForce(gravDirection);

			Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
			Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

			transform.rotation = slerp;
		}
	}
	
	//Check if the character is touching the floor in some way.
	private void CheckGrounded() {
		isGrounded = false;
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
					isGrounded = true;
				}
			}
		}
	}
	
	private Vector3 currentMomentum;
	private Vector3 currentDirection;

	//Handle movement and abilities
	private void Movement() {
		if (isInGravity) {
			//TODO Rigidbody.MovePosition doesn't work on a moving object. Find an alternative for that for physics.
			//rb.MovePosition(moveDir * (moveSpeed * sprintFactor * Time.deltaTime));
			transform.Translate(moveDir * (moveSpeed * sprintFactor * Time.deltaTime));
			Debug.Log("velocity on movement: " + rb.velocity);

			if (isGrounded) {
				//rb.velocity = Vector3.zero; //Reset the velocity
				if (Input.GetKey(KeyCode.Space)) { //Check if trying to jump
					//rb.velocity += Vector3.up * jumpSpeed; //Apply an upward velocity to jump
				}
			} /*else {
				// Check if player is trying to change forward/backward movement while jumping/falling
	            if (!Mathf.Approximately(forwardInput, 0f)) {
	                // Override just the forward velocity with player input at half speed
	                Vector3 verticalVelocity = Vector3.Project(rb.velocity, Vector3.up);
	                rb.velocity = verticalVelocity + velForward * moveSpeed / 2f + velStrafe * moveSpeed / 2f;
	            }
	        }*/
		} else {
			Vector3 moveDirection = Vector3.zero;

			if (Input.GetKey(KeyCode.E)) { 
				rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 5f * Time.fixedDeltaTime);
			} else {
				// Check for input to activate the thrusters
				if (Input.GetKey(KeyCode.W))
					moveDirection += transform.forward;
				if (Input.GetKey(KeyCode.S))
					moveDirection -= transform.forward;
				if (Input.GetKey(KeyCode.A))
					moveDirection -= transform.right;
				if (Input.GetKey(KeyCode.D))
					moveDirection += transform.right;
				if (Input.GetKey(KeyCode.Space))
					moveDirection += transform.up;
				if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
					moveDirection -= transform.up;

				// Apply the relative movement
				rb.AddForce(moveDirection.normalized * moveSpeedSpace * Time.deltaTime * 10);
			}
		}
	}

	public void AssignGravityToDroid(GravityBase gravity) {
		Debug.Log("Droid entering gravity");
		isInGravity = true;
		gravitySource = gravity;
	}

	public GravityBase CurrentGravitySource() {
		return gravitySource;
	}

	public void ExitGravity() {
		gravitySource = null;
		isInGravity = false;
		transform.parent = StationController.Instance.transform;
		SetVelocity();
		Debug.Log("Droid exiting gravity. Current vel: " + rb.velocity);
	}
	
	private Vector3 lastPosition;
	
	private void LateUpdate()
	{
		if (isInGravity)
		{
			// Calculate the current velocity when the object is a child
			lastPosition = transform.position;
			//rb.velocity = currentVelocity;
		}
	}

	private float maxSpeed = 5f;
	private void SetVelocity() {
		rb.velocity = (transform.position - lastPosition) / Time.deltaTime;
		float currentSpeed = rb.velocity.magnitude;

		// If the current speed exceeds the maximum speed, clamp the velocity to the maximum value
		if (currentSpeed > maxSpeed)
		{
			// Calculate the desired velocity vector with the maximum speed
			Vector3 desiredVelocity = rb.velocity.normalized * maxSpeed;

			// Apply the clamped velocity to the Rigidbody
			rb.velocity = desiredVelocity;
		}
	}
}

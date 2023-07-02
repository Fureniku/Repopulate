using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DroidController : MonoBehaviour {

	[SerializeField] private PreviewItem heldItem;
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float heldRotation = 0;
	
	[SerializeField] private Rigidbody rb;
	[SerializeField] private CapsuleCollider capsuleCollider;
	
	[Header("Control settings")]
	[Tooltip("Move speed in meters/second")]
	[SerializeField] private float moveSpeed = 5f;
	[Tooltip("Multiplier on forward move speed when sprinting")]
	[SerializeField] private float sprintFactor = 2f;
	[Tooltip("Maximum slope the character can jump on")]
	[Range(5f, 60f)]
	[SerializeField] private float slopeLimit = 45f;
	[Tooltip("Upward speed to apply when jumping in meters/second")]
	[SerializeField] private float jumpSpeed = 4f;
	
	private Vector3 moveDir;
	
	private bool isGrounded { get; set; }

	private bool isDroidActive = false; //Whether this is the currently controlled droid. Not to be confused with playeractive which locks the camera etc

	void Awake() {
		UpdateSelection();
		rb = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
	}

	void Update() {
		if (isDroidActive) {
			if (Input.GetKeyDown(KeyCode.R)) {
				heldRotation += 90f;
				if (heldRotation > 360) heldRotation -= 360f;
			}
		}
		
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
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
		Vector3 gravity = new Vector3(0, -9.8f, 0);
		Vector3 direction = Vector3.Normalize(Vector3.zero - transform.position);
		float magnitude = gravity.magnitude;
		Vector3 gravDirection = direction * magnitude;
		
		rb.AddForce(gravDirection);

		Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
		Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

		transform.rotation = slerp;
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
	
	//Handle movement and abilities
	private void Movement() {
		rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * moveSpeed * sprintFactor * Time.deltaTime);
		
		if (isGrounded) {
			rb.velocity = Vector3.zero; //Reset the velocity
			if (Input.GetKey(KeyCode.Space)) { //Check if trying to jump
				Debug.Log("You say jump, I say how high");
				rb.velocity += Vector3.up * jumpSpeed; //Apply an upward velocity to jump
			}
		} /*else {
            // Check if player is trying to change forward/backward movement while jumping/falling
            if (!Mathf.Approximately(forwardInput, 0f)) {
                // Override just the forward velocity with player input at half speed
                Vector3 verticalVelocity = Vector3.Project(rb.velocity, Vector3.up);
                rb.velocity = verticalVelocity + velForward * moveSpeed / 2f + velStrafe * moveSpeed / 2f;
            }
        }*/
	}
}

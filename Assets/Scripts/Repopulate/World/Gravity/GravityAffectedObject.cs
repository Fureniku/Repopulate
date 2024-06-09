using System.Collections.Generic;
using UnityEngine;

public class GravityAffectedObject : MonoBehaviour
{
    [SerializeField] private GravityBase gravitySource;
    [SerializeField] private Rigidbody rb;
    
    [Tooltip("The speed at which the object rotates to correct orientation when entering gravity")]
    [Range(1f, 10f)]
    [SerializeField] private float _gravitationalCorrectionSpeed = 5f;
    [Tooltip("The gravitational terminal velocity of the object in standard earth-level gravity")]
    [SerializeField] private float _terminalVelocity = 5f;
    
    private List<GravityBase> currentGravities = new();

    public GravityBase GravitySource => gravitySource;
    public bool IsInGravity { get; private set; }
    public bool IsInElevator { get; private set; } = false;
    
    private Vector3 lastPosition; //The last known position when in gravity, used for transitioning velocity to out-of-gravity
    
    void Awake()
    {
        IsInGravity = gravitySource != null;
    }

    public void UpdateGravity() {
        Vector3 pos = transform.position;
        if (currentGravities.Count > 0) {
            if (!IsInGravity) {
                EnterGravity();
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
            IsInGravity = true;
        } else {
            if (IsInGravity) {
                gravitySource = null;
                transform.parent = StationController.Instance.transform;
                rb.ClampVelocity(_terminalVelocity);
            }
            IsInGravity = false;
        }

        if (gravitySource != null && gravitySource.IsWithinGravitationalEffect(pos)) {
            Vector3 direction = gravitySource.GetPullDirection(pos);
            Vector3 gravDirection = gravitySource.GetPull(pos);
	
            rb.AddForce(gravDirection);

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -direction) * transform.rotation;
            Quaternion slerp = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _gravitationalCorrectionSpeed);

            transform.rotation = slerp;
        }
    }
    
    private void LateUpdate() {
        if (IsInGravity) {
            lastPosition = transform.position;
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        GravityBase gravity = other.GetComponent<GravityBase>();

        if (gravity != null) {
            IsInGravity = true;
            currentGravities.Add(gravity);
        }
    }

    private void OnTriggerExit(Collider other) {
        GravityBase gravity = other.GetComponent<GravityBase>();

        if (gravity != null) {
            currentGravities.Remove(gravity);
        }
    }

    private void OnTriggerStay(Collider other) {
        GravityLift gravLift = other.GetComponent<GravityLift>();

        if (gravLift != null) {
            IsInElevator = true;
            if (CurrentlyHasExternalForce()) {
                gravLift.HandleForces(rb);
            }
        } else {
            IsInElevator = false;
        }
    }

    protected virtual bool CurrentlyHasExternalForce() {
        return false;
    }

    protected virtual void EnterGravity() {
        transform.localScale = Vector3.one;
    }
}

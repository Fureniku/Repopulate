using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class GravityLift : MonoBehaviour {

    [SerializeField] private float strength = 10f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float radius = 5f;
    
    [SerializeField] private float centerForce = 10f;
    [SerializeField] private float maxSlowdownRangeFactor = 0.1f; // Controls how close to the center the slowdown range extends
    [SerializeField] private float dampingForce = 5f;

    private CapsuleCollider cldr;

    void Awake() {
        cldr = GetComponent<CapsuleCollider>();
        cldr.isTrigger = true;
    }

    public void HandleForces(Rigidbody rb) {
	    float slowRadius = cldr.radius * maxSlowdownRangeFactor;
	    Vector3 localPosition = transform.InverseTransformPoint(rb.transform.position);
	    localPosition.y = 0f; //Only get X/Z distance, so distance to horizontal centre

	    float distanceToCenter = localPosition.magnitude - slowRadius;

	    Vector3 forceVector = Vector3.zero;

	    if (distanceToCenter > 0f) {
		    // Calculate the slowdown factor based on the distance and inner threshold
		    float slowdownFactor = Mathf.Clamp01(distanceToCenter / slowRadius);
		    // Apply the force towards the centre only if we're outside the inner threshold
		    if (slowdownFactor > 0f) {
			    float centerForceMagnitude = centerForce * slowdownFactor;

			    Vector3 centerForceVector = new Vector3(-localPosition.x, 0f, -localPosition.z).normalized * centerForceMagnitude;

			    // Calculate the horizontal damping force for stabilization
			    Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
			    Vector3 horizontalDampingForceVector = -horizontalVelocity * dampingForce;

			    forceVector += centerForceVector + horizontalDampingForceVector;
		    }
	    }

	    // Apply the upward force on the local Y axis
	    Vector3 upwardForceVector = Vector3.up * strength;
	    forceVector += upwardForceVector;

	    // Apply combined forces
	    rb.AddForce(forceVector, ForceMode.Force);
    }

    private void OnValidate() {
        cldr = GetComponent<CapsuleCollider>();
        cldr.center = new Vector3(0, height / 2f, 0);
        cldr.height = height;
        cldr.radius = radius;
    }

    private void OnDrawGizmos() {
	    DrawGravitationalArea(transform.position, Vector3.zero, Vector3.up * height);
	}

	private void DrawGravitationalArea(Vector3 pos, Vector3 localBelowPoint, Vector3 localAbovePoint) {
		Gizmos.color = Color.cyan;
	    Vector3 directionAB = localBelowPoint - localAbovePoint;

	    float angleIncrement = 2 * Mathf.PI / 24;

	    Vector3[] last = null;

	    for (int i = 0; i < 25; i++) { // 25; 24 for the loop and one extra to make sure it's closed
	        float angle = i * angleIncrement;
	        Vector3 minPos = Vector3.zero;
	        Vector3 maxPos = new Vector3(0, 0, radius);

	        // Apply rotation to the points in local space
	        Vector3 point1 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
	        Vector3 point2 = pos + transform.TransformVector(localAbovePoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
	        Gizmos.DrawLine(point1, point2);

	        Vector3 pointB1 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos);
	        Vector3 pointB2 = pos + transform.TransformVector(localBelowPoint + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos);
	        Gizmos.DrawLine(pointB1, pointB2);

	        Gizmos.DrawLine(point2, pointB2);

	        if (last != null) {
	            Gizmos.DrawLine(point1, last[0]);
	            Gizmos.DrawLine(point2, last[1]);
	            Gizmos.DrawLine(pointB1, last[2]);
	            Gizmos.DrawLine(pointB2, last[3]);
	        }

	        last = new[] { point1, point2, pointB1, pointB2 };
	    }
	}
}

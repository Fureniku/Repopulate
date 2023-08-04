using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourceCylinder : GravityBase {

	//[SerializeField] private Vector3 direction;
	[SerializeField] private float aboveDistance = 5;
	[SerializeField] private float belowDistance = 5;

	[SerializeField] float radius = 10f;
	
	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		Vector3 gravityDirection = transform.up; // Use the plane's normal as gravity direction
		return (inverseGravity ? gravityDirection : -gravityDirection).normalized; //Swapped compared to other gravity systems
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		gizmoCol = Color.red;

		// Transform the pointToCheck to local coordinates of the cylinder's transform
		Vector3 localPointToCheck = transform.InverseTransformPoint(pulledObject);

		// Check if the point is within the height range of the cylinder
		if (localPointToCheck.y < -belowDistance || localPointToCheck.y > aboveDistance) {
			return false;
		}

		// Check if the point is within the circular base of the cylinder
		float distanceFromCenter = new Vector2(localPointToCheck.x, localPointToCheck.z).magnitude;
		if (distanceFromCenter > radius) {
			return false;
		}
		gizmoCol = Color.green;
		return true;
	}

	private void OnDrawGizmos() {
	    Gizmos.color = gizmoCol;
	    Vector3 pos = transform.position;

	    // Calculate positions in local coordinates
	    Vector3 localBelowPoint = Vector3.up * -belowDistance;
	    Vector3 localAbovePoint = Vector3.up * aboveDistance;
	    
	    Vector3 belowPoint = pos + transform.TransformVector(localBelowPoint);
	    Vector3 abovePoint = pos + transform.TransformVector(localAbovePoint);
	    
	    // Draw gravity line direction
	    Gizmos.color = Color.red;
	    DrawGizmoArrow(abovePoint, belowPoint);
	    
	    DrawGravitationalArea(pos, localBelowPoint, localAbovePoint);
	}

	private void DrawGravitationalArea(Vector3 pos, Vector3 localBelowPoint, Vector3 localAbovePoint) {
		Gizmos.color = gizmoCol;
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

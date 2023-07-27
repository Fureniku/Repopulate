using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourcePlane : GravityBase {

	//[SerializeField] private Vector3 direction;
	[SerializeField] private float aboveDistance = 5;
	[SerializeField] private float belowDistance = 5;

	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		Vector3 gravityDirection = transform.up; // Use the plane's normal as gravity direction
		return (inverseGravity ? gravityDirection : -gravityDirection).normalized; //Swapped compared to other gravity systems
	}

	private void OnDrawGizmos() {
		Gizmos.color = gizmoCol;
		Vector3 pos = transform.position;
		Vector3 endPoint = pos + transform.up.normalized * aboveDistance;
		Gizmos.DrawRay(closestPoint, endPoint - pos);
		Gizmos.DrawSphere(closestPoint, 0.25f);
		
		//Draw gravity line direction
		Gizmos.color = Color.cyan;
		
		Vector3 belowPoint = pos + (transform.up*-1).normalized * belowDistance;
		Vector3 abovePoint = pos + transform.up.normalized * aboveDistance;
		Gizmos.DrawLine(belowPoint, abovePoint);
		Gizmos.DrawSphere(belowPoint, 0.25f);
		Gizmos.DrawSphere(abovePoint, 0.25f);
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		GetClosestPoint(pulledObject);
		
		float distanceToGravityCentre = Vector3.Dot(transform.up.normalized, (pulledObject - closestPoint));
		distanceToGravityCentre /= transform.up.normalized.magnitude;

		bool inRange = distanceToGravityCentre >= (belowDistance*-1)  && distanceToGravityCentre <= aboveDistance; //TODO wrong

		gizmoCol = inRange ? Color.green : Color.red;
		
		return inRange;
	}

	private void GetClosestPoint(Vector3 pulledObject) {
		float distance = Vector3.Dot(Vector3.Normalize(transform.up), pulledObject - transform.position);
		closestPoint = pulledObject - distance * Vector3.Normalize(transform.up);
	}
}

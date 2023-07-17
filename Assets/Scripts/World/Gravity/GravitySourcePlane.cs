using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourcePlane : GravitySource {

	[SerializeField] private Vector3 direction;

	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		return Vector3.Normalize(direction) * (inverseGravity ? -1 : 1);
	}

	private void OnDrawGizmos() {
		Gizmos.color = gizmoCol;
		Vector3 pos = transform.position;
		Vector3 endPoint = pos + direction.normalized * 10f;
		Gizmos.DrawRay(closestPoint, endPoint - pos);
		Gizmos.DrawSphere(closestPoint, 0.25f);
		
		//Draw gravity line direction
		Gizmos.color = Color.cyan;
		
		endPoint = pos + direction.normalized * 10f;
		Gizmos.DrawRay(pos, endPoint - pos);
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		GetClosestPoint(pulledObject);

		float distanceToGravityCentre = Vector3.Distance(pulledObject, closestPoint);
		bool inRange = distanceToGravityCentre >= minEffectDistance && distanceToGravityCentre <= maxEffectDistance;

		gizmoCol = inRange ? Color.green : Color.red;
		
		return inRange;
	}

	private void GetClosestPoint(Vector3 pulledObject) {
		float distance = Vector3.Dot(Vector3.Normalize(direction), pulledObject - transform.position);
		closestPoint = pulledObject - distance * Vector3.Normalize(direction);
	}
}

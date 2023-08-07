using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourceCube : GravityBase {

	[SerializeField] private BoxCollider box;

	[SerializeField] private float aboveDistance = 5;
	[SerializeField] private float belowDistance = 5;

	[SerializeField] float radius = 10f;
	
	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		Vector3 gravityDirection = transform.up; // Use the plane's normal as gravity direction
		return (inverseGravity ? gravityDirection : -gravityDirection).normalized; //Swapped compared to other gravity systems
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		gizmoCol = Color.red;

		gizmoCol = Color.green;
		return true;
	}

	private void OnDrawGizmos() {
	    Gizmos.color = gizmoCol;
	    Vector3 pos = transform.position;

	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySourceCube : GravityBase {

	//[SerializeField] private Vector3 direction;
	[SerializeField] private BoxCollider box;

	
	public override Vector3 GetPullDirection(Vector3 pulledObject) {
		Vector3 gravityDirection = transform.up; // Use the plane's normal as gravity direction
		return (inverseGravity ? gravityDirection : -gravityDirection).normalized; //Swapped compared to other gravity systems
	}
	
	public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		gizmoCol = Color.red;

		//TODO check if positions within box
		gizmoCol = Color.green;
		return true;
	}

	private void OnDrawGizmos() {
	    Gizmos.color = gizmoCol;
	    Vector3 pos = transform.position;

	}
}

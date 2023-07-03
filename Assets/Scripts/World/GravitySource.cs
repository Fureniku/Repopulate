using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour {

	[Tooltip("The strength and direction of the gravity")]
	[SerializeField] private Vector3 gravity = new Vector3(0, -9.8f, 0);
	[Tooltip("The position gravity pulls towards")]
	[SerializeField] private Vector3 gravitySource = new Vector3(0, 0, 0);
	[Tooltip("If the object is closer than this, gravity has no effect")]
	[SerializeField] private float minEffectDistance;
	[Tooltip("If the object is further than this, gravity has no effect")]
	[SerializeField] private float maxEffectDistance;

	[Tooltip("If true, gravity pulls to a central point and Gravity Source B is ignorerd. Else, it pulls to a line defined by Gravity Source B.")]
	[SerializeField] private bool singlePoint;

	[Tooltip("The second point. Gravity's centre is a line between Gravity Source and this.")]
	[SerializeField] private Vector3 gravitySourceB;

	private float gravityDistance;
	
	void Awake() {
		if (!singlePoint) {
			gravityDistance = Vector3.Distance(gravitySource, gravitySourceB);
		}
	}

	public Vector3 GetGravity() {
		return gravity;
	}

	public float GetMagnitude() {
		return gravity.magnitude;
	}
	
	//The direction of pull, without strength
	public Vector3 GetPullDirection(Transform pulledObject) {
		return Vector3.Normalize(GetGravitationalLineOrigin(pulledObject) - pulledObject.position);
	}

	//The direction and strength of the gravitational pull
	public Vector3 GetPull(Transform pulledObject) {
		return GetPullDirection(pulledObject) * gravity.magnitude;
	}

	public bool IsSinglePoint() {
		return singlePoint;
	}

	private Vector3 closestPoint;

	public Vector3 GetGravitationalLineOrigin(Transform pulledObject) {
		if (singlePoint) { //If we're only using one point it's always going to be the same place; the point of gravity.
			return gravitySource;
		}
		Vector3 d = (gravitySourceB - gravitySource).normalized;

		Vector3 v = pulledObject.position - gravitySource;

		float scalarProjection = Vector3.Dot(v, d);

		//Limit the gravitational position to within the source line
		if (scalarProjection <= 0) { return gravitySource; }
		if (scalarProjection >= gravityDistance) { return gravitySourceB; }

		closestPoint = gravitySource + scalarProjection * d;

		return closestPoint;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		if (singlePoint) {
			Gizmos.DrawSphere(gravitySource, 0.5f);
		} else {
			Gizmos.DrawLine(gravitySource, gravitySourceB);
			Gizmos.DrawSphere(closestPoint, 0.25f);
		}
	}
}

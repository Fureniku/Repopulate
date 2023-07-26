using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour {
	
	[Tooltip("The strength and direction of the gravity")]
	[SerializeField] private Vector3 gravity = new Vector3(0, -9.8f, 0);
	[Tooltip("If true, gravity pulls to a central point and Gravity Source B is ignorerd. Else, it pulls to a line defined by Gravity Source B.")]
	[SerializeField] private bool singlePoint;
	[Tooltip("The position gravity pulls towards")]
	[SerializeField] private Vector3 gravitySource = new Vector3(0, 0, 0);
	[Tooltip("The second point. Gravity's centre is a line between Gravity Source and this.")]
	[SerializeField] private Vector3 gravitySourceB;
	[Tooltip("If the object is closer than this, gravity has no effect")]
	[SerializeField] private float minEffectDistance;
	[Tooltip("If the object is further than this, gravity has no effect")]
	[SerializeField] private float maxEffectDistance;
	[Tooltip("If true, the force of gravity will push away from this point instead of pulling towards it. Used for gravity rings.")]
	[SerializeField] protected bool inverseGravity = false;
	
	private float gravityDistance;
	
	//Debug:
	protected Vector3 closestPoint; //The last returned closest point, to update the gizmo position
	protected Color gizmoCol = Color.magenta; //Variable to change gizmo colour when player is within gravitational range
	
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

	//Adjust gravitational positions to move with the gravitational object
	private Vector3 GravitySourceA() {
		return gravitySource + transform.position;
	}
	
	private Vector3 GravitySourceB() {
		return gravitySourceB + transform.position;
	}
	
	//The direction of pull, without strength. Overriden in plane
	public virtual Vector3 GetPullDirection(Vector3 pulledObject) {
		return Vector3.Normalize(GetGravitationalLineOrigin(GetScalarProjection(pulledObject)) - pulledObject) * (inverseGravity ? -1 : 1);
	}

	//The direction and strength of the gravitational pull
	public Vector3 GetPull(Vector3 pulledObject) {
		return GetPullDirection(pulledObject) * gravity.magnitude;
	}

	public virtual bool IsWithinGravitationalEffect(Vector3 pulledObject) {
		float scalarProjection = GetScalarProjection(pulledObject);
		if (!singlePoint) {
			//Limit the gravitational position to within the source line
			if (scalarProjection <= 0 || scalarProjection >= gravityDistance) {
				gizmoCol = Color.red;
				return false;
			}
		}

		float distanceToGravityCentre = Vector3.Distance(pulledObject, GetGravitationalLineOrigin(scalarProjection));
		bool inRange = distanceToGravityCentre >= minEffectDistance && distanceToGravityCentre <= maxEffectDistance;

		gizmoCol = inRange ? Color.green : Color.red;
		
		return inRange;
	}

	public bool IsSinglePoint() {
		return singlePoint;
	}

	private float GetScalarProjection(Vector3 pulledObject) {
		Vector3 d = (GravitySourceB() - GravitySourceA()).normalized;

		Vector3 v = pulledObject - GravitySourceA();

		return Vector3.Dot(v, d);
	}

	public Vector3 GetGravitationalLineOrigin(float scalarProjection) {
		if (singlePoint) { //If we're only using one point it's always going to be the same place; the point of gravity.
			return GravitySourceA();
		}
		
		//Limit the gravitational position to within the source line
		if (scalarProjection <= 0) { return GravitySourceA(); }
		if (scalarProjection >= gravityDistance) { return GravitySourceB(); }

		closestPoint = GravitySourceA() + scalarProjection * (GravitySourceB() - GravitySourceA()).normalized;

		return closestPoint;
	}

	private void OnDrawGizmos() {
		Gizmos.color = gizmoCol;
		if (singlePoint) {
			Gizmos.DrawSphere(GravitySourceA(), 0.5f);
		} else {
			Gizmos.DrawLine(GravitySourceA(), GravitySourceB());
			Gizmos.DrawSphere(closestPoint, 0.25f);
			
			DrawGravitationalArea();
		}
	}

	private void DrawGravitationalArea() {
		Vector3 directionAB = (GravitySourceB() - GravitySourceA()).normalized;
		Vector3 perpendicular = Vector3.Cross(directionAB, Vector3.up).normalized;

		float angleIncrement = 2 * Mathf.PI / 24;

		Vector3[] last = null;
		
		for (int i = 0; i < 25; i++) { //25; 24 for the loop and one extra to make sure its closed
			float angle = i * angleIncrement;
			Vector3 minPos = minEffectDistance * perpendicular;
			Vector3 maxPos = maxEffectDistance * perpendicular;
			Vector3 point1 = GravitySourceA() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos;
			Vector3 point2 = GravitySourceA() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos;
			DrawGizmoArrow(point1, point2);
			
			Vector3 pointB1 = GravitySourceB() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * minPos;
			Vector3 pointB2 = GravitySourceB() + Quaternion.AngleAxis(angle * Mathf.Rad2Deg, directionAB) * maxPos;
			DrawGizmoArrow(pointB1, pointB2);
			
			Gizmos.DrawLine(point1, pointB1);
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

	private void DrawGizmoArrow(Vector3 target, Vector3 tail) {
		Gizmos.DrawLine(target, tail);
		Gizmos.DrawSphere(target, 0.25f);
	}
}

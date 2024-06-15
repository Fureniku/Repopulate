using UnityEngine;

namespace Repopulate.Physics.Gravity {
	public class GravitySourcePoint : GravityBase {
	
		[Tooltip("The position gravity pulls towards")]
		[SerializeField] private Vector3 gravitySource = new Vector3(0, 0, 0);
		[Tooltip("If the object is closer than this, gravity has no effect")]
		[SerializeField] private float minEffectDistance;
		[Tooltip("If the object is further than this, gravity has no effect")]
		[SerializeField] private float maxEffectDistance;

		//Adjust gravitational positions to move with the gravitational object
		private Vector3 GravitySource() {
			return gravitySource + transform.position;
		}

		//The direction of pull, without strength. Overriden in plane
		public override Vector3 GetPullDirection(Vector3 pulledObject) {
			return Vector3.Normalize(GetGravitationalLineOrigin(GetScalarProjection(pulledObject)) - pulledObject) * (_inverseGravity ? -1 : 1);
		}

		public override bool IsWithinGravitationalEffect(Vector3 pulledObject) {
			float scalarProjection = GetScalarProjection(pulledObject);

			float distanceToGravityCentre = Vector3.Distance(pulledObject, GetGravitationalLineOrigin(scalarProjection));
			bool inRange = distanceToGravityCentre >= minEffectDistance && distanceToGravityCentre <= maxEffectDistance;

			return inRange;
		}
	
		private float GetScalarProjection(Vector3 pulledObject) {
			Vector3 d = GravitySource().normalized;
			Vector3 v = pulledObject - GravitySource();
			return Vector3.Dot(v, d);
		}

		public Vector3 GetGravitationalLineOrigin(float scalarProjection) {
			return GravitySource();
		}

		private void OnDrawGizmos() {
			gizmoCol.a = 0.3f;
			Gizmos.color = gizmoCol;
			Gizmos.DrawSphere(GravitySource(), minEffectDistance);
			Gizmos.DrawSphere(GravitySource(), maxEffectDistance);
		}
	}
}
